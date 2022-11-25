using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiningMgr : MonoBehaviour
{
    [SerializeField] ParticleSystem particlesDestroyed;
    [SerializeField] ParticleSystem particlesHit;
    [SerializeField] RectTransform BlockHealthCanvasRT;
    [SerializeField] Transform pickHitPoint;
    [SerializeField] GameObject unknownBlock;
    [SerializeField] GameObject startBlock;
    [SerializeField] Transform blocksParent;
    [SerializeField] float blocksOffset;
    [SerializeField] float blocksLimit = 5;
    [SerializeField] RectTransform blockInfoUIRT;
    int blockCurrentHp;
    block currentBlock;
    int currentLayerID = 0;
    int currentBlockID = 0;
    List<Transform> spawnedBlocks = new List<Transform>();
    bool isRebirthing = false;

    float animTimer = 1f;
    float animTotalTime = 0.2f;

    public static System.Action<int, int> BlockDestroyed;
    public static System.Action NewBlockSpawned;
    public static System.Action PickaxeHit;

    private void Awake()
    {
        AutoClicker.Tap += OnTouched;
        DataMgr.Rebirthed += RebirthReset;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLayerID = PlayerPrefs.GetInt("savedLayer", 0);
        currentBlockID = PlayerPrefs.GetInt("savedBlock", 0);
        currentBlock = DataMgr.instance.GetBlocksInfo()[currentLayerID].blocks[currentBlockID];
        blockCurrentHp = PlayerPrefs.GetInt("savedHp", currentBlock.hp);

        GenerateBlocks();
        UpdateBlockUI();
    }

    void RebirthReset()
    {
        isRebirthing = true;

        currentLayerID = 0;
        currentBlockID = 0;
        PlayerPrefs.SetInt("savedLayer", 0);
        PlayerPrefs.SetInt("savedBlock", 0);
        currentBlock = DataMgr.instance.GetBlocksInfo()[currentLayerID].blocks[currentBlockID];
        blockCurrentHp = PlayerPrefs.GetInt("savedHp", currentBlock.hp);
        print(blockCurrentHp);
        spawnedBlocks.Clear();

        GenerateBlocks();
        UpdateBlockUI();

        isRebirthing = false;
    }

    void UpdateBlockUI()
    {
        UIMGR.instance.UpdateBlockUI(blockCurrentHp, currentBlock);
    }

    int getRandomAngle() 
    {
        int a = Random.Range(0, 4);
        a *= 90;
        return a;
    }

    Quaternion getRandomRot()
    {
        Vector3 rot;
        rot.x = getRandomAngle();
        rot.y = getRandomAngle();
        rot.z = getRandomAngle();
        return Quaternion.Euler(rot);
    }

    void GenerateBlocks() 
    {
        //Destroy initial scene blocks
        for (int i = 0; i < blocksParent.childCount; i++)
        {
            Destroy(blocksParent.GetChild(i).gameObject);
        }

        Vector3 pos = Vector3.zero;
        Vector3 rot = Vector3.zero;

        GameObject prefab = DataMgr.instance.GetBlocksInfo()[currentLayerID].blocks[currentBlockID].prefab;
        if (PlayerPrefs.GetInt("blocksMined", 0) == 0)
        {
            prefab = startBlock;
        }
       
        spawnedBlocks.Add(Instantiate(prefab, pos, Quaternion.Euler(rot), blocksParent).transform);

        prefab = unknownBlock;
        for (int i = 0; i < blocksLimit; i++)
        {
            pos.y += blocksOffset;
            spawnedBlocks.Add(Instantiate(prefab, pos, Quaternion.Euler(rot), blocksParent).transform);
        }
    }

    void OnBlockDestoyed() 
    {
        DataMgr.instance.AddToBlocksMined();

        BlockDestroyed?.Invoke(currentLayerID, currentBlockID);
        AudioMgr.instance.PlayAudioBlockDestroyed();

        Destroy(spawnedBlocks[0].gameObject);
        spawnedBlocks.RemoveAt(0);

        //Instantiate(particlesDestroyed, Vector3.zero, Quaternion.identity);


        // Destroy particles
        ParticleSystem.MainModule _main = particlesDestroyed.main;
        _main.startColor = DataMgr.instance.GetCurrentBlock().particleCol;
        particlesDestroyed.Emit(10);

        SpawnUnknownBlock();
        RevealTopBlock();
        AnimateBlocksMovement();
        NewBlockSpawned?.Invoke();
    }

    void AnimateBlocksMovement()
    {
        animTimer = 0f;
    }

    void RevealTopBlock()
    {
        Destroy(spawnedBlocks[0].gameObject);

        Vector3 pos = Vector3.zero;
        
        // Update new block variables
        currentBlockID = DataMgr.instance.GetRandomBlockIDAtLayer(currentLayerID);
        currentLayerID = DataMgr.instance.GetCurrentLayerIndex();

        // Save block
        PlayerPrefs.SetInt("savedBlock", currentBlockID);
        PlayerPrefs.SetInt("savedLayer", currentLayerID);

        currentBlock = DataMgr.instance.GetBlocksInfo()[currentLayerID].blocks[currentBlockID];
        GameObject prefab = currentBlock.prefab;

        PlayerPrefs.SetInt("savedHp", currentBlock.hp);
        blockCurrentHp = currentBlock.hp;

        spawnedBlocks[0] = Instantiate(prefab, pos, getRandomRot(), blocksParent).transform;
        UpdateBlockUI();

        blockInfoUIRT.localScale = new Vector3(1f, 1f, 1f);
    }

    void SpawnUnknownBlock()
    {
        GameObject prefab = unknownBlock;
        Vector3 rot = Vector3.zero;
        Vector3 pos = Vector3.zero;

        pos.y += blocksOffset * (blocksLimit+1);

        spawnedBlocks.Add(Instantiate(prefab, pos, Quaternion.Euler(rot), blocksParent).transform);
    }

    void OnTouched()
    {
        // When backpack full
        if (DataMgr.instance.IsIniniteBackpackEquipped() == false 
            && DataMgr.instance.GetBackpackItemsCount() == DataMgr.instance.GetCurrentBackpack().capacity)
        {
            UIMGR.instance.ShowFullBackpackPanel();
            AudioMgr.instance.PlayAudioFullBackpack();
            return;
        }

        // Particles
        ParticleSystem.MainModule _main = particlesHit.main;
        _main.startColor = DataMgr.instance.GetCurrentBlock().particleCol;
        particlesHit.Emit(4);

        // Broadcast message
        PickaxeHit?.Invoke();

        // Block animation on hit
        spawnedBlocks[0].localScale = new Vector3(0.9f, 0.9f, 0.9f);
        BlockHealthCanvasRT.localScale = new Vector3(0.023f, 0.023f, 0.023f);

        // Hit audio
        AudioMgr.instance.PlayAudioHit();

        int pickDmg = 0;
        if (DataMgr.instance.isUltimatePickaxeEquipped())
        {
            pickDmg = DataMgr.instance.GetPremiumPickaxe().power;
        }
        else
        {
            pickDmg = DataMgr.instance.GetCurrentPickaxe().power;
        }

        blockCurrentHp -= pickDmg;

        if (blockCurrentHp <= 0) 
        {
            OnBlockDestoyed();
        }

        PlayerPrefs.SetInt("savedHp", blockCurrentHp);
        UpdateBlockUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRebirthing)
        {
            return;
        }

        Vector3 currentSize = spawnedBlocks[0].localScale;
        Vector3 normalSize = new Vector3(1, 1, 1);
        spawnedBlocks[0].localScale = Vector3.Lerp(currentSize, normalSize, Time.deltaTime * 4);

        currentSize = BlockHealthCanvasRT.localScale;
        normalSize = new Vector3(0.024f, 0.024f, 0.024f);
        BlockHealthCanvasRT.localScale = Vector3.Lerp(currentSize, normalSize, Time.deltaTime * 4);

        currentSize = blockInfoUIRT.localScale;
        normalSize = new Vector3(0.9f, 0.9f, 0.9f);
        blockInfoUIRT.localScale = Vector3.Lerp(currentSize, normalSize, Time.deltaTime * 4);

        if (animTimer < animTotalTime)
        {
            animTimer += Time.deltaTime;

            Vector3 oldPos = new Vector3(0, blocksOffset, 0);

            for (int i = 0; i < spawnedBlocks.Count; i++)
            {
                Vector3 newPos = oldPos;
                newPos.y -= blocksOffset;

                spawnedBlocks[i].position = Vector3.Lerp(oldPos, newPos, animTimer/animTotalTime);
                oldPos.y += blocksOffset;
            }
        }

        if (Input.touchCount > 0) 
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    {
                        OnTouched();
                    }
                }
            }
        }
    }
}
