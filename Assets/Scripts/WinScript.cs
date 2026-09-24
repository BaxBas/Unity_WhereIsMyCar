//using UnityEngine;

//public class WinScript : MonoBehaviour
//{
//    public GameObject winScreenPanel;

//    public void CheckWinCondition()
//    {
//        // 1. Find all active car objects currently alive in the scene
//        // (Objects destroyed by bombs will automatically be excluded)
//        CarState[] activeCars = FindObjectsByType<CarState>(FindObjectsSortMode.None);

//        // If all cars were destroyed by bombs, handle game over / no win
//        if (activeCars.Length == 0)
//        {
//            Debug.Log("No cars left to place!");
//            return;
//        }

//        // 2. Check if every existing car is placed
//        bool allCarsPlaced = true;
//        foreach (CarState car in activeCars)
//        {
//            if (!car.isPlaced)
//            {
//                allCarsPlaced = false;
//                break; // Stop checking as soon as one unplaced car is found
//            }
//        }

//        // 3. Trigger Win Screen if every remaining car is placed correctly
//        if (allCarsPlaced)
//        {
//            Debug.Log("YOU WIN! All surviving cars are correctly placed.");
//            ShowWinScreen();
//        }
//    }

//    private void ShowWinScreen()
//    {
//        if (winScreenPanel != null)
//        {
//            winScreenPanel.SetActive(true);
//        }
//    }
//}
