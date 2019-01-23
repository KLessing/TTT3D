namespace AugmentedImage
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;

    // Controller for the Augmented Image
    public class AugmentedImageController : MonoBehaviour
    {
        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public AugmentedImageVisualizer AugmentedImageVisualizerPrefab;
        
        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;

        // The Game Object which contains the complete User Interface
        public GameObject UserInterfaceCanvas;

        private Dictionary<int, AugmentedImageVisualizer> m_Visualizers
            = new Dictionary<int, AugmentedImageVisualizer>();

        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);

            // Create visualizers and anchors for updated augmented images that are tracking and do not previously
            // have a visualizer. Remove visualizers for stopped images.
            foreach (var image in m_TempAugmentedImages)
            {
                AugmentedImageVisualizer visualizer = null;
                m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                if (image.TrackingState == TrackingState.Tracking && visualizer == null)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    visualizer = (AugmentedImageVisualizer)Instantiate(AugmentedImageVisualizerPrefab, anchor.transform);
                    visualizer.Image = image;
                    m_Visualizers.Add(image.DatabaseIndex, visualizer);
                }
                else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
                {
                    m_Visualizers.Remove(image.DatabaseIndex);
                    GameObject.Destroy(visualizer.gameObject);
                }
            }

            // Check each image for the trackingstate
            // (only one Gamefield image in the database for now)
            foreach (var visualizer in m_Visualizers.Values)
            {
                // When the image is tracked
                if (visualizer.Image.TrackingState == TrackingState.Tracking)
                {                         
                    // Hide the fit to scan overlay 
                    // and show the UI Interface                                  
                    FitToScanOverlay.SetActive(false);
                    UserInterfaceCanvas.SetActive(true);
                    return;
                }
            }
            
            // Otherwise
            // Show the fit-to-scan overlay if there are no images that are Tracking.
            // And hide the UI Interface accordingly
            FitToScanOverlay.SetActive(true);
            UserInterfaceCanvas.SetActive(false);
        }
    }
}
