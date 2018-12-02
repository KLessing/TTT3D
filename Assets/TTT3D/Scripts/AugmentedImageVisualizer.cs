//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
namespace GoogleARCore.Examples.AugmentedImage
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

        // The GameField for the Tokens to visualize above the Augmented Image of the GameField
        public Dictionary<Field, Token> GameField = new Dictionary<Field, Token>()
        {
            { Field.TopLeft, Token.SmallCross1 },
            { Field.Middle, Token.MediumCross2 },
            { Field.BottomRight, Token.LargeCross1 }
        };

        // All Tokens
        public GameObject SmallCross1;
        public GameObject MediumCross1;
        public GameObject LargeCross1;

        public GameObject SmallCross2;
        public GameObject MediumCross2;
        public GameObject LargeCross2;

        public GameObject SmallCircle1;
        public GameObject MediumCircle1;
        public GameObject LargeCircle1;

        public GameObject SmallCircle2;
        public GameObject MediumCircle2;
        public GameObject LargeCircle2;

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

        // Returns the Gameobject for a Token enum
        private GameObject GetTokenObject(Token token)
        {
            switch (token)
            {
                case Token.SmallCross1: return SmallCross1;
                case Token.MediumCross1: return MediumCross1;
                case Token.LargeCross1: return LargeCross1;

                case Token.SmallCircle1: return SmallCircle1;
                case Token.MediumCircle1: return MediumCircle1;
                case Token.LargeCircle1: return LargeCircle1;

                case Token.SmallCross2: return SmallCross2;
                case Token.MediumCross2: return MediumCross2;
                case Token.LargeCross2: return LargeCross2;

                case Token.SmallCircle2: return SmallCircle2;
                case Token.MediumCircle2: return MediumCircle2;
                case Token.LargeCircle2: return LargeCircle2;

                default: return null;
            }
        }

        // The Unity Update method.
        public void Update()
        {
            // Always deactivate all Tokens first
            // (Otherwise Tokens are visible even when not used)     
            foreach (Token token in Enum.GetValues(typeof(Token)))
            {
                GetTokenObject(token).SetActive(false);
            }                  

            // Show Tokens while tracking the image
            if (Image != null && Image.TrackingState == TrackingState.Tracking)
            {
                // Calcuate the center of the indivdual Fields on the Gamefield                
                float centerX = Image.ExtentX / 3;
                float centerY = Image.ExtentZ / 3;

                foreach (var placement in GameField)
                {
                    GetTokenObject(placement.Value).transform.localPosition = GetPositionVector(placement.Key, centerX, centerY);
                    GetTokenObject(placement.Value).SetActive(true);
                }
            }
        }

    }
}
