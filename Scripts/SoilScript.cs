using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilScript : MonoBehaviour
{

    public bool isWet;
    public float timeToDry = 120;
    public Material materialDry;
    public Material materialWet;

    public int seedIndex;
    public int cropStage;
    public GameObject cropObject;

    private MeshRenderer thisMeshRenderer;
    private float dryCooldown = 0;
    private int oldCropStage;
    private float growInterval = 1;
    private float growCooldown = 0;
    private float growChance = 1.1f;


    void Awake(){
        thisMeshRenderer = GetComponent<MeshRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        dryCooldown = timeToDry;
    }

    // Update is called once per frame
    void Update()
    {
        //update material
        thisMeshRenderer.material = isWet ? materialWet : materialDry;

        //Dry soil
        if(isWet){
            dryCooldown -= Time.deltaTime;
            if(dryCooldown <= 0){
                isWet = false;
            }
        }
        //Update crops
        if(oldCropStage != cropStage){

            if(cropObject != null){
                Destroy(cropObject);
        }
            if(cropStage > 0){
                var gm = GameManager.Instance;
                var prefabs = seedIndex == 1 ? gm.beetPrefabs : gm.pumpkinPrefabs;
                var cropPrefab =  prefabs[cropStage -1];

                var position = transform.position;
                var rotation = cropPrefab.transform.rotation * Quaternion.Euler(Vector3.up * Random.Range(0, 360));
                cropObject = Instantiate(cropPrefab, position, rotation);
            }
        oldCropStage = cropStage;

        //Grow crops
        if(!isEmpty() && !isFinished()){
            if((growCooldown -= Time.deltaTime) <= 0){
            growCooldown = growInterval;

                var realChance = growChance;
                if(isWet){
                    realChance *= 10f;
                }

                if(Random.Range(0f, 1f) < growChance){
                cropStage++;
            }
        }
        }
    }
    }

    public void Water(){
        isWet = true;
        dryCooldown = timeToDry;
    }

    public bool isEmpty(){
        return cropStage == 0;
    }

    public bool isFinished(){
        return cropStage == 5;
    }

    public void Seed(int index){
        if(!isEmpty()) return;

        //set vars
        seedIndex = index;
        cropStage = 1;
    }
}

