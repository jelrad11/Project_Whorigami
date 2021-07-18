using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spritemaskanimation : MonoBehaviour
{

    #region VOiD1 Gaming

    // 1. You can directly attach this Script to any GameObject and see it in action
    // 2. All the variables given here are tweakable, You can change them as per you Game Requirement
    // 3. Nearly each line of the Script is Documented so that it can be accessed easily by anyone
    // 4. This Script contains- Approach 1 -> Animation Derived from the Animator that is animating the Sprites in the Sprite Renderer
    //    Approach 2-> Custom Animation Script to animate all the assigned sprites in the Sprite Array 
    // 5. Coroutine has been used to animate the Sprite Mask, you can change it as per your needs
    // 6. An Additional Scene has been included in the Project File in order to help you getting started 

    #endregion VOiD1 Gaming

    // Here we need to first check, whether we require to derive the Animation from the Animator or use custom script to animate the Sprite Mask.
    [Header("Get Animation From Sprite Renderer?")]
    [Tooltip("Derive the Animation from the Sprite Renderer?")]
    public bool DeriveFromSpriteRenderer; // Boolean to check if the Animation should be derived from the Animator animating the Sprite Renderer or else individually animate the Sprites assigned
    [Tooltip("Reference to the Animated Sprite Renderer")]
    [Header("Reference to Sprite Renderer")]
    public SpriteRenderer spriteRenderer; // Reference to the Sprite Renderer which is being animated by the Animator

    // Now here we go for custom animation with the sprites assigned to the array while using the Time between Frames
    [Header("Sprite Array")]
    [Header("Use individual Sprite instead?")]
    [Tooltip("All the Sprites that is to be Animated")]
    public Sprite[] sprite; // Reference to all the Sprites that is to be animated
    [Header("Reference to Sprite Mask")]
    [Tooltip("Reference to the Sprite Mask")]
    public SpriteMask spritemask; // Reference to the Sprite Mask
    [Header("Time between Frames")]
    [Tooltip("Total Time between Frames if it is not derived from any Animator")]
    public float TimeBetweenFrames; // Time Between Frames
    void Start()
    {
        StartCoroutine(AnimateSpriteMask()); // We start the animation as soon as the Game runs
    }
    // This is the Coroutine that Animates the Sprite Mask as per the Sprite Sequence assigned. However, this is the core logic, you can use your custom way of running the loop 
    IEnumerator AnimateSpriteMask()
    {
        // While statement is required to loop it continously 
        while (true)
        {
            // Now first we check if we want to derive the animation from the Animator animating the Sprite in Sprite Renderer or we want to use a custom script for Animation
            if(DeriveFromSpriteRenderer == false) // First we check if DeriveFromSpriteRenderer is false
            {
                // Then we go over a iteration of all the sprites present in the Sprite Array using the For Loop
                for (int i = 0; i < sprite.Length; i++)
                {
                    // And we assign the Sprite of the Spritemask as the current index of the Sprite Array
                    spritemask.sprite = sprite[i];
                    // We wait for a certain period of Time between Frames
                    yield return new WaitForSeconds(TimeBetweenFrames);
                }
                yield return new WaitForEndOfFrame();
            }
            // Now we check for the else statement that runs if the animation is to be derived from Animator, i.e, DeriveFromSpriteRenderer is true
            else
            {
                // Then as we know, the Animator is animating the Sprites in the Sprite Renderer as per the key frame Animation, here we would rather just check for the current
                // sprite in the Sprite mask and compare it to the sprite in the Sprite Renderer and hence change it accordingly each frame
                if(spritemask.sprite != spriteRenderer.sprite)
                {
                    // So here we change the sprite of the Sprite Mask if it doesn't matches with the sprite in the Sprite Renderer
                    spritemask.sprite = spriteRenderer.sprite;

                }
                yield return new WaitForEndOfFrame();
            }

        }
        
    }
}
