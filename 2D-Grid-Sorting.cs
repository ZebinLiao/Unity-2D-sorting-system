using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class CharacterMovementController : MonoBehaviour 
{
    [SerializeField]
    private float speed = 1.3f;
    [SerializeField]
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform tr;
    private bool isJumping;
    private int layer;
    private int currentLayer; 
    private GameObject grid;

    private GameObject subGrid;

    private GameObject sortGrid;
    private Vector3 currentPosition;
    private Vector3 currentSortPoint;
    private TileBase lastTile;

    private (Vector3,Vector3,Vector3,Vector3) sortCorners;
    public Tilemap emptyTilemap;

    public TileBase sortTile;
    
    private bool onSlab;

    private bool onStair;

    private bool onBackRightStair;
    private bool onBackLeftStair;

    float stairMod;

    Vector3Int previousStairPos;
    Vector3Int currentStairPos;






    void Awake(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        tr = gameObject.GetComponent<Transform>();
        grid = GameObject.Find("/Grid");
        subGrid = GameObject.Find("/subGrid");
        sortGrid = GameObject.Find("/SortGrid");
        //print(grid.transform.childCount);
        currentPosition = new Vector3(tr.position.x, tr.position.y - 0.16f, 0);
        currentSortPoint = new Vector3(tr.position.x, tr.position.y - 0.25f, 0);
        //UpdateLayer();
        onSlab = false;
        currentLayer = 0;
    }
    private void OnMovement(InputValue value){
        movement = value.Get<Vector2>();
        if(movement.x != 0 || movement.y != 0){
            if(movement.x != 0 && movement.y != 0){
                animator.SetBool("isWalking", true);
            }
            else{
                animator.SetFloat("X", movement.x);
                animator.SetFloat("Y", movement.y);

                animator.SetBool("isWalking", true);
            }
        }
        else{
            animator.SetBool("isWalking", false);
        }
    }

    private void OnJump(){
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName){
         //Author: Isaac Dart, June-13.
         Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
         foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
         return null;
     }
    public TileBase CheckStair(){
        for(int n = 0; n < grid.transform.childCount; n++){
            Transform iterMap = grid.transform.GetChild(n);
            Tilemap iterTmap = iterMap.GetComponent<Tilemap>();
            Vector3Int cellPosLayer1 = iterTmap.WorldToCell(new Vector3(currentPosition.x, currentPosition.y, 0));
            TileBase currentIterTile = iterTmap.GetTile(cellPosLayer1);

            // if(currentIterTile){
            //     if(currentIterTile.name.EndsWith("Stair")){
            //         //print("holydamzlmaolololol");
            //         //print(iterTmap.name);
            //         //return int.Parse(iterTmap.name.Substring(iterTmap.name.Length - 1));
            //         //print("hoiiiiiii");
            //         currentStairPos = cellPosLayer1;
            //         print("0");
            //         return null;
            //         //return currentIterTile;
            //     }
            // }

            if(n + 1 < grid.transform.childCount){
                Transform iterMapBelow = grid.transform.GetChild(n+1);
                Tilemap iterTMapBelow = iterMapBelow.GetComponent<Tilemap>();
                Vector3Int cellPosLayer2 = iterTmap.WorldToCell(new Vector3(currentPosition.x, currentPosition.y - 0.16f, 0));
                Vector3Int cellPosLayer3 = new Vector3Int(cellPosLayer1.x - 1, cellPosLayer1.y, 0);
                Vector3Int cellPosLayer4 = new Vector3Int(cellPosLayer1.x, cellPosLayer1.y - 1, 0);

                TileBase currentIterTileBelow2 = iterTMapBelow.GetTile(cellPosLayer2);
                TileBase currentIterTileBelow3 = iterTMapBelow.GetTile(cellPosLayer3);
                TileBase currentIterTileBelow4 = iterTMapBelow.GetTile(cellPosLayer4);
                TileBase currentIterTileBelow5 = iterTMapBelow.GetTile(cellPosLayer1);

                int layerInt = int.Parse(iterTMapBelow.name.Substring(iterTmap.name.Length - 1)) - 1;

                print(currentIterTileBelow5);
                

                if(currentIterTileBelow2){
                    if(currentIterTileBelow2.name.EndsWith("Stair")){
                        string txt = "Layer" + (layerInt + 2);
                        GameObject sMapObject = getChildGameObject(sortGrid, txt);
                        if(sMapObject){
                            Tilemap sMap = sMapObject.GetComponent<Tilemap>();
                            sMap.GetTile(cellPosLayer2);
                            if(!sMap.GetTile(cellPosLayer2) && onStair){
                                //print("hoiiiiiii");
                                currentStairPos = cellPosLayer2;
                                return currentIterTileBelow2;
                            }
                        }
                        else{
                            if(onStair){
                                //print("idfk");
                                currentStairPos = cellPosLayer2;
                                return currentIterTileBelow2;
                            }
                        }
                        //print(iterMapBelow.name);
                    }
                }
                //broken
                if(currentIterTileBelow3){
                    //print("true");
                    if(currentIterTileBelow3.name.EndsWith("Stair")){
                        string txt = "Layer" + (layerInt + 2);
                        GameObject sMapObject = getChildGameObject(sortGrid, txt);
                        if(sMapObject){
                            Tilemap sMap = sMapObject.GetComponent<Tilemap>();
                            sMap.GetTile(cellPosLayer3);
                            if(!sMap.GetTile(cellPosLayer3) && onStair){
                                //print("hoiiiiiii");
                                currentStairPos = cellPosLayer3;
                                return currentIterTileBelow3;
                            }
                        }
                        else{
                            if(onStair){
                                currentStairPos = cellPosLayer3;
                                //print("idfk");
                                return currentIterTileBelow3;
                            }
                        }
                        //print(iterMapBelow.name);
                    }
                }
                //broken
                if(currentIterTileBelow4){
                    if(currentIterTileBelow4.name.EndsWith("Stair")){
                        print("hello");
                        string txt = "Layer" + (layerInt + 2);
                        print(txt);
                        GameObject sMapObject = getChildGameObject(sortGrid, txt);
                        if(sMapObject){
                            Tilemap sMap = sMapObject.GetComponent<Tilemap>();
                            sMap.GetTile(cellPosLayer4);
                            if(!sMap.GetTile(cellPosLayer4) && onStair){
                                //print("hoiiiiiii");
                                currentStairPos = cellPosLayer4;
                                print("hi");
                                return currentIterTileBelow4;
                            }
                        }
                        else{
                            if(onStair){
                                //print("idfk");
                                currentStairPos = cellPosLayer4;
                                print("hewoo");
                                return currentIterTileBelow4;
                            }
                        }
                        print("here");
                        print(currentIterTileBelow5);
                    }
                    //print("end");
                }
                
                if(currentIterTileBelow5){
                    print("yes");
                    if(currentIterTileBelow5.name.EndsWith("Stair")){
                        for(int i = 0; i < sortGrid.transform.childCount; i++){
                            string txt = "Layer" + (layerInt + i + 1);
                            GameObject sMapObject = getChildGameObject(sortGrid, txt);
                            if(sMapObject){
                                Tilemap sMap = sMapObject.GetComponent<Tilemap>();
                                sMap.GetTile(cellPosLayer1);
                                if(sMap.GetTile(cellPosLayer1)){
                                    //print("hoiiiiiii");
                                    return null;
                                }
                            }
                        }
                        print("yes");
                        //if(onStair){
                            //print("idfk");
                            currentStairPos = cellPosLayer3;
                            return currentIterTileBelow5;
                        //}
                        
                        //print("hoiiiiiii");
                    }
                }
            }
        }
        print("huh");
        return null;
    }

    //  private void setFrontTiles(Vector3Int pos, Tilemap map, Tilemap sMap){
    //     Vector3Int p = pos;
    //     Vector3Int frontLeft = new Vector3Int(p.x-1, p.y,0);
    //     Vector3Int frontRight = new Vector3Int(p.x, p.y-1,0);
    //     while(noFront(pos,map) == true){
    //         sMap.SetTile(frontLeft, map.GetTile(frontLeft));
    //     }
    //  }

    //  private bool noFront(Vector3Int pos, Tilemap map){
    //     if(
    //         map.GetTile(new Vector3Int(pos.x-1, pos.y,0)) ==null &&
    //         map.GetTile(new Vector3Int(pos.x, pos.y-1,0)) ==null &&
    //         map.GetTile(new Vector3Int(pos.x-1, pos.y-1,0)) ==null){
    //         return true;
    //     }
    //     else return false;
    //  }

    private void UpdateLayer(){
        bool layerChanged = false;
        //print(CheckStair());
        for(int i = 0; i < grid.transform.childCount; i++){

            Transform map = grid.transform.GetChild(i);
            Transform subMap = subGrid.transform.GetChild(i);
            Transform lol = grid.transform.GetChild(2);
            //print("lol");
            //print(lol.name);
            //print("lol");
            Tilemap tmap = map.GetComponent<Tilemap>();
            Tilemap subTmap = subMap.GetComponent<Tilemap>();
            //print(tmap.name);

            TilemapCollider2D collider1 = map.GetComponent<TilemapCollider2D>();
            TilemapCollider2D subCollider1 = subMap.GetComponent<TilemapCollider2D>();

            //enable the top collider grid and disable the ones that are not on the current level
            GameObject topColliderGrid = GameObject.Find("/CollisionGrid");
            print(i);
            Transform colliderMap = topColliderGrid.transform.GetChild(i);
            TilemapCollider2D topCollider = colliderMap.GetComponent<TilemapCollider2D>();

            if(colliderMap.name == "TopCollision" + (currentLayer + 1)){
                if(!onBackRightStair && !onBackLeftStair){
                    topCollider.enabled = true;
                }
            }
            else{
                topCollider.enabled = false;
            }

            if(collider1.name == "Layer" + (currentLayer + 2) && !onSlab){
                collider1.enabled = true;
                subCollider1.enabled = true;
            }
            else{
                collider1.enabled = false;
                subCollider1.enabled = false;
            }

            // GameObject topColliderGrid = GameObject.Find("/CollisionGrid");
            // for(int j = 0; j < topColliderGrid.transform.childCount; j++){
            //     Transform colliderMap = topColliderGrid.transform.GetChild(j);
            //     TilemapCollider2D topCollider = colliderMap.GetComponent<TilemapCollider2D>();
            //     //print(colliderMap.name);
            //     if(colliderMap.name == "TopCollision" + (currentLayer + 1)){
            //         if(!onBackRightStair && !onBackLeftStair){
            //             topCollider.enabled = true;
            //             collider1.enabled = false;
            //         }
            //     }
            //     else{
            //         topCollider.enabled = false;
            //     }
            //     for(int k = 0; k < grid.transform.childCount; k++){
            //         Transform map2 = grid.transform.GetChild(k);
            //         TilemapCollider2D collider2 = map2.GetComponent<TilemapCollider2D>();
            //         if(map2.name == "Layer" + (currentLayer + 2) && !onSlab){
            //             //print("lol");
            //             //print(map2.name);
            //             collider2.enabled = true;
            //         }
            //         else{
            //             collider2.enabled = false;
            //         }
            //     }
            // }

            //print(tmap.name);
            //print(onSlab);

            if(tmap.name == "Layer" + (currentLayer + 1)){
                //print(tmap.name);
                collider1.enabled = false;
            }


            Vector3Int cellPosLayer1 = tmap.WorldToCell(new Vector3(currentPosition.x, currentPosition.y - 0.16f, 0));
            Vector3Int cellPosLayer2 = tmap.WorldToCell(new Vector3(currentSortPoint.x, currentSortPoint.y + 0.08f, 0));

            TileBase currentTile = tmap.GetTile(cellPosLayer1);
            if(tmap.name == "Layer" + (currentLayer + 1)){
                //print(currentTile);
                if(CheckStair() || onStair){
                    currentTile = CheckStair();
                }
                if(currentTile){
                    //print(tile.name);
                    //layer = tmap.GetComponent<TilemapRenderer>().sortingOrder;
                    //print(layer);

                    ////print(currentTile.name);

                    if(currentTile.name == "halfsSlab" && !onSlab){

                        // sMap.SetTile(cellPosLayer2, null);
                        tr.position = new Vector2(tr.position.x, tr.position.y - 0.08f);
                        onSlab = true;
                        
                        string txt = "Layer" + (currentLayer + 2);
                        GameObject sMapObject = getChildGameObject(sortGrid, txt);
                        Destroy(sMapObject);

                        layerChanged = true;
                        currentLayer--;
                    }

                    //print("heh");
                    //print(CheckStair());
                    //print(onStair);

                    if(CheckStair() && !onStair){

                        //Destroy(sMapObject);
                        //currentLayer++;
                        onStair = true;
                        print("lmao");
                        print(CheckStair());
                        print("lmao");
                    }

                    // if(!CheckStair() && onStair){
                    //     print("lol");
                    //     for(int n = 0; n < grid.transform.childCount; n++){
                    //         Transform iterMap = grid.transform.GetChild(n);
                    //         Tilemap iterTmap = iterMap.GetComponent<Tilemap>();
                    //         TileBase currentIterTile = iterTmap.GetTile(cellPosLayer1);
                    //         if(currentIterTile){
                    //             currentLayer = int.Parse(iterTmap.name.Substring(iterTmap.name.Length - 1)) - 1;
                    //             print(currentLayer);
                    //             onStair = false;
                    //             break;
                    //         }
                    //     }
                    // }
                }
                print(onStair);
                if(!CheckStair() && onStair){
                    print("lol");
                    for(int n = 0; n < grid.transform.childCount; n++){
                        Transform iterMap = grid.transform.GetChild(n);
                        Tilemap iterTmap = iterMap.GetComponent<Tilemap>();
                        TileBase currentIterTile = iterTmap.GetTile(cellPosLayer1);
                        if(currentIterTile){
                            layerChanged = true;
                            currentLayer = int.Parse(iterTmap.name.Substring(iterTmap.name.Length - 1)) - 1;
                            print(currentLayer);
                            onStair = false;
                            //print("hehehehehehhe");
                            //print(CheckStair());
                            break;
                        }
                    }
                    //print(CheckStair());
                }
                if(CheckStair() && onStair && (currentStairPos != previousStairPos)){
                    for(int n = 0; n < grid.transform.childCount; n++){
                        Transform iterMap = grid.transform.GetChild(n);
                        Tilemap iterTmap = iterMap.GetComponent<Tilemap>();
                        TileBase currentIterTile = iterTmap.GetTile(cellPosLayer1);
                        if(currentIterTile){
                            if(currentIterTile.name.EndsWith("Stair")){
                                if(currentLayer != int.Parse(iterTmap.name.Substring(iterTmap.name.Length - 1)) - 1){
                                    layerChanged = true;
                                    currentLayer = int.Parse(iterTmap.name.Substring(iterTmap.name.Length - 1)) - 1;
                                    break;
                                }
                            }
                        }
                    }
                    print("idfk");
                }
                //print("heh");
            }

            if(layerChanged == true){break;}

            if(tmap.name == "Layer" + (currentLayer + 2)){
                string txt = "Layer" + (currentLayer + 2);

                GameObject mmap = getChildGameObject(sortGrid, txt);

                Tilemap mMap;

                if(mmap == null){
                    mMap = Instantiate(emptyTilemap, new Vector3(0,0,0), new Quaternion(0,0,0,0), sortGrid.transform) as Tilemap;
                    mMap.name = txt;
                    mMap.GetComponent<TilemapRenderer>().sortingOrder = tmap.GetComponent<TilemapRenderer>().sortingOrder;
                }

                GameObject sMapObject = getChildGameObject(sortGrid, txt);
                Tilemap sMap = sMapObject.GetComponent<Tilemap>();

                for(int j = 0; j < 31; j++){
                    for(int k = 0; k < 31; k++){
                        Vector3Int tilePos = new Vector3Int(cellPosLayer2.x - (15-j), cellPosLayer2.y - (15-k), 0);
                        //sMap.SetTile(tilePos, null);

                        TileBase sortTile2 = tmap.GetTile(tilePos);
                        TileBase sortSubTile2 = subTmap.GetTile(tilePos);
                        Vector3 pos = tmap.CellToWorld(tilePos);

                        if(j <= 2 || j >= 28 || k <= 2 || k >= 28){
                            sMap.SetTile(tilePos, null);
                        }
                        if(pos.y > currentSortPoint.y){
                            sMap.SetTile(tilePos, null);
                        }
                        // if(k <= 1 || k >= 7){
                        //     sMap.SetTile(tilePos, null);
                        // }
                        else{
                            if(pos.y < currentSortPoint.y){
                                sMap.SetTile(tilePos, sortTile2);
                                if(sortSubTile2){
                                    sMap.SetTile(tilePos, sortSubTile2);
                                }
                                
                            }
                        }
                        if(onSlab){
                            sMap.SetTile(tilePos, null);
                        }
                        if(onStair){
                            sMap.SetTile(tilePos, null);
                        }
                        if(onBackRightStair || onBackLeftStair){
                            sMap.SetTile(tilePos, null);
                        }
                    }
                }

                TileBase currentTile2 = tmap.GetTile(cellPosLayer2);
                TilemapCollider2D collider = map.GetComponent<TilemapCollider2D>();
                // if(onSlab){
                //     cellPosLayerSlab = tmap.WorldToCell(new Vector3(currentSortPoint.x, currentSortPoint.y, 0));
                //     currentTile2 = tmap.GetTile(cellPosLayerSlab);
                // }

                if(onSlab){
                    collider.enabled = false;
                    Vector3Int cellPosLayerSlab = tmap.WorldToCell(new Vector3(currentSortPoint.x, currentSortPoint.y, 0));
                    currentTile2 = tmap.GetTile(cellPosLayerSlab);

                    sMap.SetTile(cellPosLayerSlab, null);
                    if(currentTile2){
                        sMap.SetTile(cellPosLayerSlab, null);
                        //print(currentTile2.name);
                    }
                    else{
                        //collider.enabled = true;
                        tr.position = new Vector2(tr.position.x, tr.position.y - 0.08f);
                        onSlab = false;
                    }
                }

                // if(CheckStair() || onStair){
                //     currentTile2 = CheckStair();
                // }

                // print(onStair);
                // print(CheckStair());

                if(currentTile2){
                    //print("hehe");
                    //print(currentTile2.name);
                    if(currentTile2.name == "halfsSlab" && !onSlab){

                        sMap.SetTile(cellPosLayer2, null);
                        tr.position = new Vector2(tr.position.x, tr.position.y + 0.08f);
                        onSlab = true;
                    }
                    if (currentTile2.name != "halfsSlab" && onSlab){
                        //collider.enabled = true;
                        tr.position = new Vector2(tr.position.x, tr.position.y + 0.08f);

                        Destroy(sMapObject);

                        layerChanged = true;

                        currentLayer++;
                        onSlab = false;
                    }
                    if(CheckStair() && !onStair){

                        Destroy(sMapObject);

                        layerChanged = true;
                        currentLayer++;
                        onStair = true;
                    }

                    if(!CheckStair() && onStair){

                        Destroy(sMapObject);

                        layerChanged = true;
                        currentLayer++;
                        onStair = true;
                    }

                    int layerInt = int.Parse(tmap.name.Substring(tmap.name.Length - 1)) - 2;

                    if(CheckStair() && onStair && currentLayer == layerInt){

                        Destroy(sMapObject);

                        layerChanged = true;
                        currentLayer++;
                        onStair = true;
                    }

                    if(currentTile2.name.EndsWith("Stair") && onStair){
                        sMap.SetTile(cellPosLayer2, null);
                    }
                    // if(CheckStair() == 999 && onStair){
                    //     print(lol);
                    //     for(int n = 0; n < grid.transform.childCount; n++){
                    //         Transform iterMap = grid.transform.GetChild(n);
                    //         Tilemap iterTmap = iterMap.GetComponent<Tilemap>();
                    //         TileBase currentIterTile = iterTmap.GetTile(cellPosLayer1);
                    //         if(currentIterTile){
                    //             currentLayer = int.Parse(iterTmap.name.Substring(iterTmap.name.Length - 1)) - 1;
                    //             break;
                    //         }
                    //     }
                    //     onStair = false;
                    // }
                    //print(currentLayer);
                    // print(layerInt);
                    // print(currentLayer);
                    // print(CheckStair());
                    // print(onStair);
                }
            }

            if(layerChanged == true){break;}

            char last_char = tmap.name[tmap.name.Length - 1];
            int last_num = last_char - '0';
            //print(currentLayer);
            if(tmap.name == "Layer" + (currentLayer + 3)){
                //print("lol");
                string txt = "Layer" + (currentLayer + 3);

                GameObject mmap = getChildGameObject(sortGrid, txt);

                Tilemap mMap;

                if(mmap == null){
                    mMap = Instantiate(emptyTilemap, new Vector3(0,0,0), new Quaternion(0,0,0,0), sortGrid.transform) as Tilemap;
                    mMap.name = txt;
                    mMap.GetComponent<TilemapRenderer>().sortingOrder = tmap.GetComponent<TilemapRenderer>().sortingOrder;
                }

                GameObject sMapObject = getChildGameObject(sortGrid, txt);
                Tilemap sMap = sMapObject.GetComponent<Tilemap>();

                for(int j = 0; j < 35; j++){
                    for(int k = 0; k < 35; k++){
                        Vector3Int tilePos = new Vector3Int(cellPosLayer2.x - (17-j) +1, cellPosLayer2.y - (17-k)+1, 0);
                        //sMap.SetTile(tilePos, null);

                        TileBase sortTile2 = tmap.GetTile(tilePos);
                        TileBase sortSubTile2 = subTmap.GetTile(tilePos);
                        Vector3 pos = tmap.CellToWorld(tilePos);

                        if(j <= 2 || j >= 32 || k <= 2 || k >= 32){
                            sMap.SetTile(tilePos, null);
                        }
                        if(pos.y > (currentSortPoint.y + 0.16)){
                            sMap.SetTile(tilePos, null);
                        }

                        else{
                            if(pos.y < currentSortPoint.y + 0.16){
                                sMap.SetTile(tilePos, sortTile2);
                                if(sortSubTile2){
                                    sMap.SetTile(tilePos, sortSubTile2);
                                }
                            }
                        }
                    }
                }
            } 

            if(last_num > currentLayer + 3){
                //print("lol");
                string txt = "Layer" + last_num;

                GameObject mmap = getChildGameObject(sortGrid, txt);

                Tilemap mMap;

                if(mmap == null){
                    mMap = Instantiate(emptyTilemap, new Vector3(0,0,0), new Quaternion(0,0,0,0), sortGrid.transform) as Tilemap;
                    mMap.name = txt;
                    mMap.GetComponent<TilemapRenderer>().sortingOrder = tmap.GetComponent<TilemapRenderer>().sortingOrder;
                }

                GameObject sMapObject = getChildGameObject(sortGrid, txt);
                Tilemap sMap = sMapObject.GetComponent<Tilemap>();

                for(int j = 0; j < 35; j++){
                    for(int k = 0; k < 35; k++){
                        Vector3Int tilePos = new Vector3Int(cellPosLayer2.x - (17-j) +1, cellPosLayer2.y - (17-k)+1, 0);
                        //sMap.SetTile(tilePos, null);

                        TileBase sortTile2 = tmap.GetTile(tilePos);
                        TileBase sortSubTile2 = subTmap.GetTile(tilePos);
                        Vector3 pos = tmap.CellToWorld(tilePos);

                        // if(j <= 2 || j >= 32 || k <= 2 || k >= 32){
                        //     sMap.SetTile(tilePos, null);
                        // }
                        // if(pos.y > (currentSortPoint.y + 0.16)){
                        //     sMap.SetTile(tilePos, null);
                        // }

                        // else{
                            // if(pos.y < currentSortPoint.y + 0.16){
                                sMap.SetTile(tilePos, sortTile2);
                                if(sortSubTile2){
                                    sMap.SetTile(tilePos, sortSubTile2);
                                }
                            // }
                        // }
                    }
                }
            } 
            lastTile = currentTile;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        //print("hhuhuhuhuhuh");
        if(collision.gameObject.name == "RightStairGrid"){
            //print("lelelelelellelel");
            onBackRightStair = true;
            ToggleCurrentLayerTopCollider(false);
            return;
        }

        if(collision.gameObject.name == "LeftStairGrid"){
            onBackLeftStair = true;
            ToggleCurrentLayerTopCollider(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.name == "RightStairGrid" || collision.gameObject.name == "LeftStairGrid"){
            onBackRightStair = false;
            onBackLeftStair = false;
            ToggleCurrentLayerTopCollider(true);

            for(int n = 0; n < grid.transform.childCount; n++){
                Transform iterMap = grid.transform.GetChild(n);
                Tilemap iterTmap = iterMap.GetComponent<Tilemap>();
                Vector3Int cellPosLayer1 = iterTmap.WorldToCell(new Vector3(currentPosition.x, currentPosition.y - 0.16f, 0));
                TileBase currentIterTile = iterTmap.GetTile(cellPosLayer1);
                if(currentIterTile){
                    if(!currentIterTile.name.EndsWith("StairBack") && !currentIterTile.name.EndsWith("Stair")){
                        currentLayer = int.Parse(iterTmap.name.Substring(iterTmap.name.Length - 1)) - 1;
                        string txt = "Layer" + (currentLayer + 1);
                        GameObject sMapObject = getChildGameObject(sortGrid, txt);
                        Destroy(sMapObject);

                        break;
                    }
                }
            }
        }
    }

    private void ToggleCurrentLayerTopCollider(bool toggle){
        GameObject topColliderGrid = GameObject.Find("/CollisionGrid");
        for(int j = 0; j < topColliderGrid.transform.childCount- 1; j++){
            Transform colliderMap = topColliderGrid.transform.GetChild(j);
            TilemapCollider2D topCollider = colliderMap.GetComponent<TilemapCollider2D>();
            if(colliderMap.name == "TopCollision" + (currentLayer + 1)){
                topCollider.enabled = toggle;
                return;
            }
        }
    }

    private void CheckInSortGrid(){
        
    }

    void FixedUpdate(){
        //rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        //  if(movement.x != 0 || movement.y != 0){

        //      rb.velocity = movement * speed;
        //      rb.velocity = 1;
        //  }

        float stairModLocal = 2.5f;

        if(movement.x < 0){
            if(onBackRightStair){
                stairMod = stairModLocal;
            }
            else if(onBackLeftStair){
                stairMod = - stairModLocal;
            }
        }
        else if(movement.x > 0){
            if(onBackRightStair){
                stairMod = - stairModLocal;
            }
            else if(onBackLeftStair){
                stairMod = stairModLocal;
            }
        }
        else{
            stairMod = 0;
        }

        if(movement.x == 0 && movement.y == 0){
            stairMod = 0;
        }

        Vector2 movementMod = new Vector2(movement.x, movement.y * 0.7f);
        Vector2 force = movementMod * speed;
        rb.AddForce(new Vector2(force.x, force.y + stairMod));
        if(onSlab){
            //currentPosition = new Vector3(tr.position.x, tr.position.y - 0.27f, 0);
        }
        currentPosition = new Vector3(tr.position.x, tr.position.y - 0.17f, 0);
        currentSortPoint = new Vector3(tr.position.x, tr.position.y - 0.252f, 0);
        //print(new Vector3(tr.position.x, tr.position.y, 0));
        //print(currentPosition);
        UpdateLayer();
        previousStairPos = currentStairPos;
        //print(currentLayer);
    }
}
