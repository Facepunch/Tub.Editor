using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Profiling;

[ExecuteInEditMode]
[AddComponentMenu("Rendering/Highlight")]
public class Highlight : MonoBehaviour
{
    //
    // We want to draw before all post processing, because we want it to 
    // get antialiased and for bloom to be applied if the HDR level is high
    //
    const CameraEvent CameraEventTarget = CameraEvent.AfterForwardAlpha;

    public static Highlight Instance;

    public bool VisualizeRenderTarget;

    public Camera TargetCamera;
    public Material ImageEffectMaterial;
    public Material MeshRenderMaterial;

    CommandBuffer commandBuffer;

    private bool isDirty;
    private int _HighlightTextureId;

    public List<Renderer> Highlighted = new List<Renderer>();

    /// <summary>
    /// Remove all rendererers from the highlighter.
    /// You'll need to call Rebuild to see changes reflected.
    /// </summary>
    public static bool ClearAll()
    {
        if ( Instance == null ) return false;
        if ( Instance.Highlighted.Count == 0 ) return false;

        Instance.Highlighted.Clear();
        Instance.isDirty = true;
        return true;
    }

    /// <summary>
    /// Add a renderer to the highlighter. You'll need to call Rebuild
    /// to see the changes reflected.
    /// </summary>
    public static bool AddRenderer( Renderer r )
    {
        if ( Instance == null ) return false;
        if ( Instance.Highlighted.Contains( r ) ) return false;

        Instance.Highlighted.Add( r );
        Instance.isDirty = true;
        return true;
    }

    /// <summary>
    /// Remove renderer to the highlighter. You'll need to call Rebuild
    /// to see the changes reflected.
    /// </summary>
    public static bool RemoveRenderer( Renderer r )
    {
        if ( Instance == null ) return false;
        if ( !Instance.Highlighted.Contains( r ) ) return false;

        Instance.Highlighted.Remove( r );
        Instance.isDirty = true;
        return true;
    }

    /// <summary>
    /// Rebuilds the internal command buffer if it's dirty.
    /// </summary>
    public static bool Rebuild( bool force = false )
    {
        if ( Instance == null ) return false;

        if ( Instance.isDirty || force )
        {
            Instance.isDirty = false;
            Instance.RebuildCommandBuffer();
            return true;
        }

        return false;
    }

    private void Start()
    {
        _HighlightTextureId = Shader.PropertyToID( "_HighlightTexture" );
    }

    /// <summary>
    /// Install the command buffer on enable
    /// </summary>
	void OnEnable()
	{
        Instance = this;
        RebuildCommandBuffer();
    }


    /// <summary>
    /// Remove the CommandBuffer on disable to disable the effect
    /// </summary>
    private void OnDisable()
    {
        Instance = null;
        ReleaseAll();
    }

    void RebuildCommandBuffer()
    {
        if ( TargetCamera == null )
            return;

        //
        // Create the buffer if it doesn't exist
        //
        if ( commandBuffer == null )
        {
            commandBuffer = new CommandBuffer();
            commandBuffer.name = "Highlight Render";
            TargetCamera.AddCommandBuffer( CameraEventTarget, commandBuffer );
        }

        //
        // Make sure it's empty, to start off
        //
        commandBuffer.Clear();

        //
        // Can't do shit without these
        //
        if ( MeshRenderMaterial == null || ImageEffectMaterial == null )
            return;

        //
        // Draw each rendererer
        //
        commandBuffer.GetTemporaryRT( _HighlightTextureId, TargetCamera.pixelWidth, TargetCamera.pixelHeight, 24, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1 );
        commandBuffer.SetRenderTarget( _HighlightTextureId );
        commandBuffer.ClearRenderTarget( true, true, Color.black );
        foreach ( var r in Highlighted )
        {
            if ( !r ) continue;

            commandBuffer.DrawRenderer( r, MeshRenderMaterial );
        }

        //
        // Draw image effect
        //
        commandBuffer.SetRenderTarget( BuiltinRenderTextureType.CameraTarget );
        commandBuffer.Blit( _HighlightTextureId, BuiltinRenderTextureType.CameraTarget, VisualizeRenderTarget ? null : ImageEffectMaterial );
        commandBuffer.ReleaseTemporaryRT( _HighlightTextureId );
    }

    /// <summary>
    /// Release anything that has been created
    /// </summary>
    public void ReleaseAll()
    {
        if ( TargetCamera != null && commandBuffer != null )
        {
            TargetCamera.RemoveCommandBuffer( CameraEventTarget, commandBuffer );
            commandBuffer = null;
        }
    }

    /// <summary>
    /// For editor only, refresh shit when messing in the inspector
    /// </summary>
    private void OnValidate()
    {
        if ( TargetCamera == null )
            TargetCamera = GetComponent<Camera>();

        ReleaseAll();
        RebuildCommandBuffer();        
    }

}
