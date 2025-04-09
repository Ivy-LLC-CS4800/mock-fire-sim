using UnityEngine;

public class SuitSwapper : MonoBehaviour
{

    public GameObject[] characterModels;
    public int initialActiveIndex = 0;
    private int currentIndex = 0;
    public bool setCameraAsChild = true; // Control if the camera should be a child
    public Vector3 cameraLocalPositionOffset = new Vector3(0f, 1.46f, 0f); // Optional camera offset
    public Quaternion cameraLocalRotationOffset = Quaternion.identity; // Optional camera rotation offset

    private Transform mainCameraTransform;

    void Start()
    {
        // Find the main camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogWarning("No Main Camera found in the scene.");
        }

        // Ensure the initial active index is within bounds
        if (initialActiveIndex < 0 || initialActiveIndex >= characterModels.Length)
        {
            Debug.LogError("Initial Active Index is out of bounds!");
            enabled = false;
            return;
        }

        // Disable all character models initially
        foreach (GameObject model in characterModels)
        {
            model.SetActive(false);
        }

        // Enable the initially active model and set the camera's parent
        if (characterModels.Length > 0 && mainCameraTransform != null)
        {
            characterModels[initialActiveIndex].SetActive(true);
            currentIndex = initialActiveIndex;
            if (setCameraAsChild)
            {
                mainCameraTransform.SetParent(characterModels[currentIndex].transform);
                mainCameraTransform.localPosition = cameraLocalPositionOffset;
                mainCameraTransform.localRotation = cameraLocalRotationOffset;
            }
        }
        else if (characterModels.Length == 0)
        {
            Debug.LogError("No character models assigned in the array!");
        }
    }

    public void SwapToNextModel()
    {
        if (characterModels.Length > 0 && mainCameraTransform != null)
        {
            // Disable the current model
            characterModels[currentIndex].SetActive(false);

            // Move to the next index (looping around if necessary)
            currentIndex = (currentIndex + 1) % characterModels.Length;

            // Enable the new current model
            characterModels[currentIndex].SetActive(true);

            // Move the main camera to the new model
            if (setCameraAsChild)
            {
                mainCameraTransform.SetParent(characterModels[currentIndex].transform);
                mainCameraTransform.localPosition = cameraLocalPositionOffset;
                mainCameraTransform.localRotation = cameraLocalRotationOffset;
            }
        }
        else if (characterModels.Length == 0)
        {
            Debug.LogError("No character models to swap to!");
        }
        else if (mainCameraTransform == null)
        {
            Debug.LogError("Main Camera not found, cannot move it.");
        }
    }

    // Example usage
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwapToNextModel();
        }
    }
}
