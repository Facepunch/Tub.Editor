using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Profiling;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
[AddComponentMenu("Rendering/Visualize Texture Density")]
public class VisualizeTexelDensity : MonoBehaviour
{
	public Shader shader = null;
	public string shaderTag = "RenderType";
	[Range( 1, 1024 )] public int texelsPerMeter = 256;
	[Range( 0, 1 )] public float overlayOpacity = 0.5f;
	public bool showHUD = true;

	private Camera mainCamera;
	private bool initialized = false;

	private int screenWidth = 0;
	private int screenHeight = 0;

	private Camera texelDensityCamera;
	private RenderTexture texelDensityRT;
	private Texture texelDensityGradTex;
	private Material texelDensityOverlayMat;

	private static VisualizeTexelDensity instance = null;
	public static VisualizeTexelDensity Instance { get { return instance; } }

	void Awake()
	{
		instance = this;
		mainCamera = GetComponent<Camera>();
	}

	void OnEnable()
	{
		mainCamera = GetComponent<Camera>();

		screenWidth = Screen.width;
		screenHeight = Screen.height;

		LoadResources();

		initialized = true;
	}

	private void OnDisable()
	{
		SafeDestroyViewTexelDensity();
		SafeDestroyViewTexelDensityRT();
		initialized = false;
	}

	void LoadResources()
	{
		if ( texelDensityGradTex == null )
		{
			texelDensityGradTex = Resources.Load( "TexelDensityGrad" ) as Texture;
		}

		if ( texelDensityOverlayMat == null )
		{
			texelDensityOverlayMat = new Material( Shader.Find( "Hidden/TexelDensityOverlay" ) ) { hideFlags = HideFlags.DontSave };
		}
	}

	void SafeDestroyViewTexelDensity()
	{
		if ( texelDensityCamera != null )
		{
			GameObject.DestroyImmediate( texelDensityCamera.gameObject );
			texelDensityCamera = null;
		}

		if ( texelDensityGradTex != null )
		{
			Resources.UnloadAsset( texelDensityGradTex );
			texelDensityGradTex = null;
		}

		if ( texelDensityOverlayMat != null )
		{
			Material.DestroyImmediate( texelDensityOverlayMat );
			texelDensityOverlayMat = null;
		}
	}

	void SafeDestroyViewTexelDensityRT()
	{
		if ( texelDensityRT != null )
		{
			Graphics.SetRenderTarget( null );
			RenderTexture.DestroyImmediate( texelDensityRT );
			texelDensityRT = null;
		}
	}

	void UpdateViewTexelDensity( bool screenResized )
	{
		if ( texelDensityCamera == null )
		{
			GameObject obj = new GameObject( "Texel Density Camera", typeof( Camera ) ) { hideFlags = HideFlags.HideAndDontSave };

			obj.transform.parent = mainCamera.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;

			texelDensityCamera = obj.GetComponent<Camera>();
			texelDensityCamera.CopyFrom( mainCamera );
			texelDensityCamera.renderingPath = RenderingPath.Forward;
			texelDensityCamera.allowMSAA = false;
			texelDensityCamera.allowHDR = false;
			texelDensityCamera.clearFlags = CameraClearFlags.Skybox;
			texelDensityCamera.depthTextureMode = DepthTextureMode.None;
			texelDensityCamera.SetReplacementShader( shader, shaderTag );
			texelDensityCamera.enabled = false;
		}

		if ( texelDensityRT == null || screenResized || !texelDensityRT.IsCreated() )
		{
			texelDensityCamera.targetTexture = null;

			SafeDestroyViewTexelDensityRT();

			texelDensityRT = new RenderTexture( screenWidth, screenHeight, 24, RenderTextureFormat.ARGB32 ) { hideFlags = HideFlags.DontSave };
			texelDensityRT.name = "TexelDensityRT";
			texelDensityRT.filterMode = FilterMode.Point;
			texelDensityRT.wrapMode = TextureWrapMode.Clamp;
			texelDensityRT.Create();
		}

		if ( texelDensityCamera.targetTexture != texelDensityRT )
		{
			texelDensityCamera.targetTexture = texelDensityRT;
		}

		Shader.SetGlobalFloat( "global_TexelsPerMeter", texelsPerMeter );
		Shader.SetGlobalTexture( "global_TexelDensityGrad", texelDensityGradTex );

		texelDensityCamera.fieldOfView = mainCamera.fieldOfView;
		texelDensityCamera.nearClipPlane = mainCamera.nearClipPlane;
		texelDensityCamera.farClipPlane = mainCamera.farClipPlane;
		texelDensityCamera.cullingMask = mainCamera.cullingMask;
	}

	bool CheckScreenResized( int width, int height )
	{
		if ( screenWidth != width || screenHeight != height )
		{
			screenWidth = width;
			screenHeight = height;
			return true;
		}
		return false;
	}

	void OnRenderImage( RenderTexture source, RenderTexture destination )
	{
		if ( initialized )
		{
			UpdateViewTexelDensity( CheckScreenResized( source.width, source.height ) );

			texelDensityCamera.Render();

			texelDensityOverlayMat.SetTexture( "_TexelDensityMap", texelDensityRT );
			texelDensityOverlayMat.SetFloat( "_Opacity", overlayOpacity );

			Graphics.Blit( source, destination, texelDensityOverlayMat, 0 );
		}
		else
		{
			Graphics.Blit( source, destination );
		}
	}

	void DrawGUIText( float x, float y, Vector2 size, string text, GUIStyle fontStyle )
	{
		fontStyle.normal.textColor = Color.black;
		GUI.Label( new Rect( x-1, y+1, size.x, size.y ), text, fontStyle );
		GUI.Label( new Rect( x+1, y-1, size.x, size.y ), text, fontStyle );
		GUI.Label( new Rect( x+1, y+1, size.x, size.y ), text, fontStyle );
		GUI.Label( new Rect( x-1, y-1, size.x, size.y ), text, fontStyle );
		fontStyle.normal.textColor = Color.white;
		GUI.Label( new Rect( x, y, size.x, size.y ), text, fontStyle );
	}

	void OnGUI()
	{
		if ( initialized && showHUD )
		{
			string tpmText = "Texels Per Meter";
			string minText = "0";
			string refText = texelsPerMeter.ToString();
			string maxText = ( texelsPerMeter << 1 ).ToString() + "+";

			float width = texelDensityGradTex.width;
			float height = texelDensityGradTex.height * 2;

			float x = ( Screen.width - texelDensityGradTex.width ) / 2;
			float y = 32;

			GL.PushMatrix();
			GL.LoadPixelMatrix( 0, Screen.width, Screen.height, 0 );
			Graphics.DrawTexture( new Rect( x - 2, y - 2, width + 4, height + 4 ), Texture2D.whiteTexture );
			Graphics.DrawTexture( new Rect( x, y, width, height ), texelDensityGradTex );
			GL.PopMatrix();

			GUIStyle fontStyle = new GUIStyle();
			fontStyle.fontSize = 13;

			Vector2 tpmSize = fontStyle.CalcSize( new GUIContent( tpmText ) );
			Vector2 minSize = fontStyle.CalcSize( new GUIContent( minText ) );
			Vector2 refSize = fontStyle.CalcSize( new GUIContent( refText ) );
			Vector2 maxSize = fontStyle.CalcSize( new GUIContent( maxText ) );

			DrawGUIText( ( Screen.width - tpmSize.x ) / 2, y - tpmSize.y - 5, tpmSize, tpmText, fontStyle );
			DrawGUIText( x, y + height + 6, minSize, minText, fontStyle );
			DrawGUIText( ( Screen.width - refSize.x ) / 2, y + height + 6, refSize, refText, fontStyle );
			DrawGUIText( x + width - maxSize.x, y + height + 6, maxSize, maxText, fontStyle );
		}
	}
}
