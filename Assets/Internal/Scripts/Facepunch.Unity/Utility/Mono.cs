using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facepunch
{
    internal static class Mono
    {
        //
        // MONO hates https, this fixes it. I don't know if it fixes it or just stops it erroring
        // so take this code pasted from stack overflow with a grain of salt.
        //
        // https://stackoverflow.com/questions/4926676/mono-https-webrequest-fails-with-the-authentication-or-decryption-has-failed
        //
        internal static void FixHttpsValidation()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ( System.Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors ) =>
            {
                bool isOk = true;

                // If there are errors in the certificate chain,
                // look at each error to determine the cause.
                if ( sslPolicyErrors != System.Net.Security.SslPolicyErrors.None )
                {
                    for ( int i = 0; i < chain.ChainStatus.Length; i++ )
                    {
                        if ( chain.ChainStatus[i].Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.RevocationStatusUnknown )
                        {
                            continue;
                        }
                        chain.ChainPolicy.RevocationFlag = System.Security.Cryptography.X509Certificates.X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan( 0, 1, 0 );
                        chain.ChainPolicy.VerificationFlags = System.Security.Cryptography.X509Certificates.X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build( (System.Security.Cryptography.X509Certificates.X509Certificate2)certificate );
                        if ( !chainIsValid )
                        {
                            isOk = false;
                            break;
                        }
                    }
                }
                return isOk;
            };
        }
    }
}
