namespace AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;
    using TTT3DTypes;

    public class AugmentedImageVisualizer : MonoBehaviour
    {
        /// The AugmentedImage to visualize.
        public AugmentedImage Image;

        // The GameController which contains the Gamefield for the Tokens
        // to visualize above the Augmented Image of the GameField
        public GameController GameControllerPrefab;

        // The Tokens to visualize above the Augmented Image of the GameField
        public GameObject[] Tokens;

        // Returns a position Vector for the Field on the Gamefield img
        private Vector3 GetPositionVector(Field field, float centerX, float centerY)
        {
            switch (field)
            {
                case Field.TopLeft: return (centerX * Vector3.left) + (centerY * Vector3.forward);
                case Field.TopMiddle: return (centerY * Vector3.forward);
                case Field.TopRight: return (centerX * Vector3.right) + (centerY * Vector3.forward);

                case Field.MiddleLeft: return (centerX * Vector3.left);
                case Field.Middle: return Vector3.zero;
                case Field.MiddleRight: return (centerX * Vector3.right);

                case Field.BottomLeft: return (centerX * Vector3.left) + (centerY * Vector3.back);
                case Field.BottomMiddle: return (centerY * Vector3.back);
                case Field.BottomRight: return (centerX * Vector3.right) + (centerY * Vector3.back);

                default: return Vector3.zero;
            }
        }

        // The Unity Update method.
        public void Update()
        {
            // Always deactivate all Tokens first
            // (Otherwise Tokens are visible even when not used)
            foreach (GameObject token in Tokens)
            {
                token.SetActive(false);
            }

            // Show Tokens while tracking the image
            if (Image != null && Image.TrackingState == TrackingState.Tracking) 
            {
                // Calcuate the center of the indivdual Fields on the Gamefield                
                float centerX = Image.ExtentX / 3;
                float centerY = Image.ExtentZ / 3;

                // Iterate through GameField Fields with Tokens
                foreach (var placement in GameControllerPrefab.GameField)
                {
                    // Get the index of the highest Token (Peek) from the GameField Field for the prefab array
                    // (select by name; GameObject comparison doesn't work)
                    int index = Array.FindIndex(Tokens, token => token.name == placement.Value.Peek().name);

                    // Set the Token on the Gamefield
                    Tokens[index].transform.localPosition = GetPositionVector(placement.Key, centerX, centerY);
                    Tokens[index].SetActive(true);
                }
            }
        }

    }
}
