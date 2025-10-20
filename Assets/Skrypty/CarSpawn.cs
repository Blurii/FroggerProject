using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject vehiclePrefab;
    public float spawnInterval = 2f;
    public Transform[] spawnPoints;
    private float timer;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnVehicle();
            timer = 0f;
        }
    }

    void SpawnVehicle()
    {
        int laneIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[laneIndex];
        GameObject vehicle = Instantiate(vehiclePrefab, spawnPoint.position, Quaternion.identity);
        VehicleMovement vehicleMovement = vehicle.GetComponent<VehicleMovement>();
        vehicleMovement.moveRight = (laneIndex == 0 || laneIndex == 2);
    }
}
