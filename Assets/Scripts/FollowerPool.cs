using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FollowerPool : MonoBehaviour
{
     public static FollowerPool _pool;
    
        public GameObject poolObject;
    
        List<GameObject> pool = new List<GameObject>();
        List<RectTransform> rect = new List<RectTransform>();

        public int size;
    
        Vector2 offset;
        int counter;
        float timer;
        public RectTransform toGo;
        Vector2 toGo_pos;
        bool boolStart;
        public float speed;
    
        List<Vector2> randomPos = new List<Vector2>();
        Vector2 pos;
    
        int count;
        
        
        void Start() {
    	_pool = this;
    	offset = new Vector2(Screen.width, Screen.height) / 2f;
    	toGo_pos = toGo.position;
    
    	for(int i = 0; i < size; i++) {
    	    GameObject copy = Instantiate(poolObject, transform.InverseTransformPoint(Vector3.zero), Quaternion.identity, transform);
    	    pool.Add(copy);
    	    rect.Add(copy.GetComponent<RectTransform>());
    	    rect[i].position = Vector2.zero + offset;
    	    randomPos.Add(Random.insideUnitCircle * 130f);
    	    copy.SetActive(false);
    	}
        }
       void MoveUp() {
       }
    
        void Update() {
        
    	if(boolStart) {
    	    timer += Time.deltaTime;
    
    	    if(timer < 0.5f) {
    		for(int i = 0; i < size; i++){
    		    LerpTo(rect[i], pos + randomPos[i], count);
    		}
    	    } else {
    		for(int i = 0; i<size; i++){
    		    LerpTo(rect[i], toGo_pos, count);
    		}
          
    	    }
    	}
        }
    
        void LerpTo(RectTransform rt, Vector2 pos, int followerCount) {
    	    rt.position = Vector2.Lerp(rt.position, pos, speed * Time.deltaTime * 5f);
    	    if(Vector2.Distance(rt.position, toGo_pos) <= 5f) {
    		rt.position = Vector2.zero + offset;
    		rt.gameObject.SetActive(false);
    		counter++;
    		if(counter >= size) {
    		    timer = 0f;
    		    counter = 0;
    		    boolStart = false;
    		}
    	    }
        }
    
        public void ActivateDeactivate(bool b, Vector3 worldPos, int foo) {
    	pos = Camera.main.WorldToScreenPoint(worldPos + Vector3.up*5f);
    	for(int i = 0; i < pool.Count ; i++) {
    	    pool[i].SetActive(b);
    	    rect[i].position = pos;
    	    randomPos[i] = Random.insideUnitCircle * 130f;
    	}
    	boolStart = true;
        }
}
