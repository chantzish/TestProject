//#undef UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadArchitect;
using RoadArchitect.Roads;
using MyNamespace;

public class test2 : MonoBehaviour
{
    #region model
    public Vector2 carLocation;
    public Vector2[] carBorders;
    #endregion

    #region view
    public GameObject carPrefab;
    public static float tangent2Yrotation(Vector3 POS)
    {
        float angle = Mathf.Atan(POS.x / POS.z) * 180 / Mathf.PI;
        if (POS.z < 0)
        {
            if (POS.x > 0) angle += 180;
            else angle -= 180; // if x<=0
        }
        return angle;
    }
    #endregion

    // Start is called before the first frame update
    void Start() //main
    {
        #region model
        Vector3[] arr = { new Vector3(608.9f, 0.0f, 541.9f), new Vector3(525.9f, 0.0f, 540.0f), new Vector3(461.8f, 0.0f, 539.7f), 
            new Vector3(388.4f, 0.0f, 538.0f), new Vector3(317.2f, 0.0f, 537.9f), new Vector3(227.0f, 0.0f, 537.1f), 
            new Vector3(154.4f, 0.0f, 537.4f), new Vector3(88.4f, 0.0f, 535.0f)};
        List<Vector3> nodes = new List<Vector3>(arr);
        RoadModel road1 = new RoadModel(nodes);
        List<RoadModel> allroads = new List<RoadModel>();
        allroads.Add(road1);
        Vector3[] arr2 = { new Vector3(529.9f, 0.0f, 448.3f), new Vector3(525.9f, 0.0f, 540.0f), new Vector3(523.9f, 0.0f, 584.8f),
            new Vector3(522.1f, 0.0f, 644.7f)};
        nodes = new List<Vector3>(arr2);
        RoadModel road2 = new RoadModel(nodes);
        allroads.Add(road2);
        var rand = new System.Random();
        var startroad = allroads[rand.Next(2)];
        // intersections
        //find intersections
        for(var i = 0; i < allroads.Count-1; i++)
        {
            for(var k = i+1; k < allroads.Count; k++)
            {
                for ( var n = 0; n < allroads[i].nodes.Count; n++ ) {
                    var node = allroads[i].nodes[n];
                    var roadknodes = allroads[k].nodes;
                    for (var m = 0; m < roadknodes.Count; m++)
                    {
                        if(node == roadknodes[m])
                        {
                            float a_1, b_1, a_2, b_2, a_3, b_3, a_4, b_4;
                            // road 1
                            Vector3 tempPOS = allroads[i].tangents[n];
                            float tempangle = Mathf.PI / 2 - Mathf.Atan(Mathf.Abs(tempPOS.x) / Mathf.Abs(tempPOS.z));
                            float temproadWidth = 10f;
                            float tempa = (temproadWidth / 2) * Mathf.Sin(tempangle);
                            float tempb = (temproadWidth / 2) * Mathf.Cos(tempangle);
                            if (tempPOS.x > 0) tempb = -1 * tempb;
                            if (tempPOS.z < 0) tempa = -1 * tempa;
                            //Vector3 middle = new Vector3(allroads[i].nodes[n].x, allroads[i].nodes[n].y, allroads[i].nodes[n].z);
                            Vector3 middle = allroads[i].nodes[n];
                            //point 1
                            Vector3 road2middle1 = new Vector3(middle.x + tempa, middle.y, middle.z + tempb);
                            a_1 = tempPOS.z / tempPOS.x;
                            b_1 = road2middle1.z - a_1 * road2middle1.x;
                            //point 2
                            Vector3 road2middle2 = new Vector3(middle.x - tempa, middle.y, middle.z - tempb);
                            a_2 = tempPOS.z / tempPOS.x;
                            b_2 = road2middle2.z - a_2 * road2middle2.x;
                            // road 2
                            tempPOS = allroads[k].tangents[m];
                            tempangle = Mathf.PI / 2 - Mathf.Atan(Mathf.Abs(tempPOS.x) / Mathf.Abs(tempPOS.z));
                            temproadWidth = 10f;
                            tempa = (temproadWidth / 2) * Mathf.Sin(tempangle);
                            tempb = (temproadWidth / 2) * Mathf.Cos(tempangle);
                            if (tempPOS.x > 0) tempb = -1 * tempb;
                            if (tempPOS.z < 0) tempa = -1 * tempa;
                            //middle = allroads[k].nodes[m];
                            //point 3
                            Vector3 road1middle1 = new Vector3(middle.x + tempa, middle.y, middle.z + tempb);
                            a_3 = tempPOS.z / tempPOS.x;
                            b_3 = road1middle1.z - a_3 * road1middle1.x;
                            //point 4
                            Vector3 road1middle2 = new Vector3(middle.x - tempa, middle.y, middle.z - tempb);
                            a_4 = tempPOS.z / tempPOS.x;
                            b_4 = road1middle2.z - a_4 * road1middle2.x;
                            // intersections
                            // 1-3
                            Vector3 inter1_3 = new Vector3();
                            inter1_3.x = (b_1 - b_3) / (a_3 - a_1);
                            inter1_3.z = a_1 * inter1_3.x + b_1;
                            // 1-4
                            Vector3 inter1_4 = new Vector3();
                            inter1_4.x = (b_1 - b_4) / (a_4 - a_1);
                            inter1_4.z = a_1 * inter1_4.x + b_1;
                            // 2-3
                            Vector3 inter2_3 = new Vector3();
                            inter2_3.x = (b_3 - b_2) / (a_2 - a_3);
                            inter2_3.z = a_3 * inter2_3.x + b_3;
                            // 2-4
                            Vector3 inter2_4 = new Vector3();
                            inter2_4.x = (b_4 - b_2) / (a_2 - a_4);
                            inter2_4.z = a_4 * inter2_4.x + b_4;
                        }
                    }
                }
            }
        }

        #endregion

        #region view

        // Terrain Creation
        TerrainData newTerrainData = new TerrainData();
        newTerrainData.heightmapResolution = 513;
        newTerrainData.baseMapResolution = 1024;
        newTerrainData.SetDetailResolution(1024,32);
        newTerrainData.size = new Vector3(1000, 600, 1000);
        GameObject newTerrainObject = Terrain.CreateTerrainGameObject(newTerrainData);

        // Road Architect Creation

        Object[] allRoadSystemObjects = GameObject.FindObjectsOfType<RoadSystem>();
        int nextCount = (allRoadSystemObjects.Length + 1);
        allRoadSystemObjects = null;

        GameObject newRoadSystemObject = new GameObject("RoadArchitectSystem" + nextCount.ToString());
        RoadSystem newRoadSystem = newRoadSystemObject.AddComponent<RoadSystem>();
        newRoadSystem.isMultithreaded = false;

        //Add road for new road system.
        //GameObject newRoad = newRoadSystem.AddRoad(false);

        GameObject masterIntersectionsObject = new GameObject("Intersections");
        masterIntersectionsObject.transform.parent = newRoadSystemObject.transform;
        
        // Create Nodes
        newRoadSystem.isAllowingRoadUpdates = false;


        List<Road> allRoadArchitectRoads = new List<Road>();
        foreach (var roadModel in allroads) {
            Road road = RoadAutomation.CreateRoadProgrammatically(newRoadSystem, ref roadModel.nodes);
            road.isSavingTerrainHistoryOnDisk = false;
            allRoadArchitectRoads.Add(road);
        }
        
        foreach (var road in allRoadArchitectRoads)
        {
            RoadAutomation.CreateIntersectionsProgrammaticallyForRoad(road);
        }
        
        newRoadSystem.isAllowingRoadUpdates = true;

        newRoadSystem.UpdateAllRoads();

        //create car

        float j = startroad.TranslateInverseParamToFloat(startroad.RoadDefKeysArray[2]);
        Vector3 carStart, POS;
        startroad.GetSplineValueBoth(j, out carStart, out POS);
        float angle = Mathf.PI / 2 - Mathf.Atan(Mathf.Abs(POS.x) / Mathf.Abs(POS.z));
        float roadWidth = 10f;
        float a = (roadWidth / 4) * Mathf.Sin(angle);
        float b = (roadWidth / 4) * Mathf.Cos(angle);
        if (POS.x > 0) b = -1 * b;
        if (POS.z < 0) a = -1 * a;
        carStart.x += a;
        carStart.z += b;
        carStart.y++;
        float yRotation = tangent2Yrotation(POS);
        Quaternion targetRotation = Quaternion.Euler(0, yRotation, 0);
        GameObject carClone = Instantiate(carPrefab, carStart, targetRotation);

        // set the car in other scripts as needed
        Speedometer[] allSpeedometerObjects = GameObject.FindObjectsOfType<Speedometer>();
        allSpeedometerObjects[0].vehicleBody = carClone.GetComponent<Rigidbody>();
        DriftCamera[] allDriftCameraObjects = GameObject.FindObjectsOfType<DriftCamera>();
        allDriftCameraObjects[0].lookAtTarget = carClone.transform.Find("CamRig").transform.Find("CamLookAtTarget").transform;
        allDriftCameraObjects[0].positionTarget = carClone.transform.Find("CamRig").transform.Find("CamPosition").transform;
        allDriftCameraObjects[0].sideView = carClone.transform.Find("CamRig").transform.Find("CamSidePosition").transform;

        /*int kFinalCount = road1.RoadDefKeysArray.Length;
        for (var kCount = 0; kCount < kFinalCount; kCount++)
        {
            var i = road1.TranslateInverseParamToFloat(road1.RoadDefKeysArray[kCount]);
            Vector3 myVect, tempPOS;
            road1.GetSplineValueBoth(i, out myVect, out tempPOS);
            (new GameObject($"road1 {kCount}: {myVect.ToString()}")).transform.position = myVect;
            float tempangle = Mathf.PI / 2 - Mathf.Atan(Mathf.Abs(tempPOS.x) / Mathf.Abs(tempPOS.z));
            float temproadWidth = 10f;
            float tempa = (temproadWidth / 2) * Mathf.Sin(tempangle);
            float tempb = (temproadWidth / 2) * Mathf.Cos(tempangle);
            if (tempPOS.x > 0) tempb = -1 * tempb;
            if (tempPOS.z < 0) tempa = -1 * tempa;
            myVect.x += tempa;
            myVect.z += tempb;
            (new GameObject($"road1 {kCount}-right: {myVect.ToString()}")).transform.position = myVect;
            myVect.x -= 2 * tempa;
            myVect.z -= 2 * tempb;
            (new GameObject($"road1 {kCount}-left: {myVect.ToString()}")).transform.position = myVect;
        }
        kFinalCount = road2.RoadDefKeysArray.Length;
        for (var kCount = 0; kCount < kFinalCount; kCount++)
        {
            var i = road2.TranslateInverseParamToFloat(road2.RoadDefKeysArray[kCount]);
            Vector3 myVect, tempPOS;
            road2.GetSplineValueBoth(i, out myVect, out tempPOS);
            (new GameObject($"road2 {kCount}: {myVect.ToString()}")).transform.position = myVect;
            float tempangle = Mathf.PI / 2 - Mathf.Atan(Mathf.Abs(tempPOS.x) / Mathf.Abs(tempPOS.z));
            float temproadWidth = 10f;
            float tempa = (temproadWidth / 2) * Mathf.Sin(tempangle);
            float tempb = (temproadWidth / 2) * Mathf.Cos(tempangle);
            if (tempPOS.x > 0) tempb = -1 * tempb;
            if (tempPOS.z < 0) tempa = -1 * tempa;
            myVect.x += tempa;
            myVect.z += tempb;
            (new GameObject($"road2 {kCount}-right: {myVect.ToString()}")).transform.position = myVect;
            myVect.x -= 2 * tempa;
            myVect.z -= 2 * tempb;
            (new GameObject($"road2 {kCount}-left: {myVect.ToString()}")).transform.position = myVect;
        }
        
        float a_1, b_1, a_2, b_2, a_3, b_3, a_4, b_4;
        var i = 1;
        // road 1
        Vector3 tempPOS = road1.tangents[i];
        float tempangle = Mathf.PI / 2 - Mathf.Atan(Mathf.Abs(tempPOS.x) / Mathf.Abs(tempPOS.z));
        float temproadWidth = 10f;
        float tempa = (temproadWidth / 2) * Mathf.Sin(tempangle);
        float tempb = (temproadWidth / 2) * Mathf.Cos(tempangle);
        if (tempPOS.x > 0) tempb = -1 * tempb;
        if (tempPOS.z < 0) tempa = -1 * tempa;
        Vector3 myVect = new Vector3(road1.nodes[i].x, road1.nodes[i].y, road1.nodes[i].z);
        //point 1
        myVect.x += tempa;
        myVect.z += tempb;
        a_1 = tempPOS.z / tempPOS.x;
        b_1 = myVect.z - a_1 * myVect.x;
        (new GameObject($"right1: {myVect.ToString()}")).transform.position = myVect;
        //point 2
        myVect.x -= 2 * tempa;
        myVect.z -= 2 * tempb;
        a_2 = tempPOS.z / tempPOS.x;
        b_2 = myVect.z - a_2 * myVect.x;
        (new GameObject($"left1: {myVect.ToString()}")).transform.position = myVect;
        // road 2
        tempPOS = road2.tangents[i];
        tempangle = Mathf.PI / 2 - Mathf.Atan(Mathf.Abs(tempPOS.x) / Mathf.Abs(tempPOS.z));
        temproadWidth = 10f;
        tempa = (temproadWidth / 2) * Mathf.Sin(tempangle);
        tempb = (temproadWidth / 2) * Mathf.Cos(tempangle);
        if (tempPOS.x > 0) tempb = -1 * tempb;
        if (tempPOS.z < 0) tempa = -1 * tempa;
        myVect = new Vector3(road2.nodes[i].x, road2.nodes[i].y, road2.nodes[i].z);
        //point 3
        myVect.x += tempa;
        myVect.z += tempb;
        a_3 = tempPOS.z / tempPOS.x;
        b_3 = myVect.z - a_3 * myVect.x;
        (new GameObject($"right2: {myVect.ToString()}")).transform.position = myVect;
        //point 4
        myVect.x -= 2 * tempa;
        myVect.z -= 2 * tempb;
        a_4 = tempPOS.z / tempPOS.x;
        b_4 = myVect.z - a_4 * myVect.x;
        (new GameObject($"left2: {myVect.ToString()}")).transform.position = myVect;
        // intersections
        // 1-3
        myVect.x = (b_1 - b_3) / (a_3 - a_1);
        myVect.z = a_1 * myVect.x + b_1;
        (new GameObject($"iter1-3: {myVect.ToString()}")).transform.position = myVect;
        // 1-4
        myVect.x = (b_1 - b_4) / (a_4 - a_1);
        myVect.z = a_1 * myVect.x + b_1;
        (new GameObject($"iter1-4: {myVect.ToString()}")).transform.position = myVect;
        // 2-3
        myVect.x = (b_3 - b_2) / (a_2 - a_3);
        myVect.z = a_3 * myVect.x + b_3;
        (new GameObject($"iter2-3: {myVect.ToString()}")).transform.position = myVect;
        // 2-4
        myVect.x = (b_4 - b_2) / (a_2 - a_4);
        myVect.z = a_4 * myVect.x + b_4;
        (new GameObject($"iter2-4: {myVect.ToString()}")).transform.position = myVect;
        */
        #endregion
    }
    public static void MoveCar(float x, float z)
    {

    }
}
