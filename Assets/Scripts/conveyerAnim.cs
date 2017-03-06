using UnityEngine;
using System.Collections;

public class conveyerAnim : MonoBehaviour {
    int materialIndex = 1;
    Vector2 uvAnimationRate = new Vector2( 0.0f, -1.0f );
    string textureName = "_MainTex";
 
    Vector2 uvOffset = Vector2.zero;
 
    void LateUpdate() 
    {
        uvOffset += ( uvAnimationRate * Time.deltaTime );
        if( GetComponent<Renderer>().enabled )
        {
            GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
        }
    }
}
