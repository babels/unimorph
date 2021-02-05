using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;



public class rancyclemorph : MonoBehaviour
{
  public bool lg = false;

  int blqnt;
  SkinnedMeshRenderer skinnedMeshRenderer;

  Mesh  skinnedMesh;
  int   bldx = 1;         // start on second morph regressing first
  int   blst = 0;         // sets first morph on awake

  int init = 0;
  public float blendSpeed = 0.03f;

  bool  blendOneFinished = false;

  List<int> shuffle;
  string[] blends;


  void dlog( string log ) {
     if( lg ); UnityEngine.Debug.Log( log );
  }


  void reshuf() {
     string lst  =  skinnedMesh.GetBlendShapeName(blst);
     dlog(lst + "is the odd mad out");

     for(int i = 0; i < shuffle.Count; i++) {
       int temp = shuffle[i];
       int randomIndex = UnityEngine.Random.Range(i, shuffle.Count);

       shuffle[i] = shuffle[randomIndex];
       int fk = shuffle[i]; 
       shuffle[randomIndex] = temp;
       string s = skinnedMesh.GetBlendShapeName(fk);

       if( s == lst) {
         if( i == 0 || i == blqnt) reshuf();  // prevent collision
         blst = i;
         dlog("located runner "+ blst + "at index " + i);
        }

       dlog(s);
     }

  }


  void Awake() {
     skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
     skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
     blqnt = skinnedMesh.blendShapeCount;
     blends = new string [blqnt];
     shuffle = new List<int>();
  }

 
  void Start()  {
                
    // load blend shape names to arr       
     string[] arr;
     arr = new string [blqnt];

     for(int i= 0; i < blqnt; i++) {
       string s = skinnedMesh.GetBlendShapeName(i);
       string log = "Blend Shape " + i + " " + s;
       dlog(log);
       arr[i] = s;
     }


     for(int i = 0; i < blqnt; i++) {
       shuffle.Add(i);
     }

    /* shuffle array in place */
     for(int i = 0; i < shuffle.Count; i++) {
       int temp = shuffle[i];
       int randomIndex = UnityEngine.Random.Range(i, shuffle.Count);

       shuffle[i] = shuffle[randomIndex];
       int fk = shuffle[i]; 
       shuffle[randomIndex] = temp;
       string s = skinnedMesh.GetBlendShapeName(fk);

       dlog(s);
     }

     int dx = shuffle[0];
     skinnedMeshRenderer.SetBlendShapeWeight(dx, 100);
     blst = 0;


  /* build string array */
  /*  System.Text.StringBuilder sb  = new System.Text.StringBuilder(100);

   sb = shuffle.Aggregate(sb, (b, d) => b.Append(d).Append('\n'));
   if( shuffle.Count > 0 ) sb.Length--;

   var str = sb.ToString();
   Debug.Log(str); */

  /* for (int i = 0; i < shuffle.Count;. i++) {}
     int t = shuffle[i];
     string s = skinnedMesh.GetBlendShapeName(t);
     Debug.Log(s);
    } */

  }
 
  void Update() {

     float wgt1;
     int msh1;
     string log;

     float blendMax = 100.0f - blendSpeed;

     msh1 = shuffle[bldx];                                    // current index
     wgt1 = skinnedMeshRenderer.GetBlendShapeWeight(msh1);    // current blend name

     int bldnxt = bldx + 1;

     int msls = shuffle[blst];      // last

    // wgt2 = skinnedMeshRenderer.GetBlendShapeWeight(msh2);
     float lst1  =  skinnedMeshRenderer.GetBlendShapeWeight(msls);

     string lst  =  skinnedMesh.GetBlendShapeName(msls);
     string msh  =  skinnedMesh.GetBlendShapeName(msh1);

  //   dlog("Current blend" + msh + "Last Blend"    + lst);
  //     dlog("Current blend" + msh + " Weight " + wgt1 + "\nLast Blend"    + lst + " Weight " + lst1);

     lst1 = skinnedMeshRenderer.GetBlendShapeWeight(msls);    // last
     wgt1 = skinnedMeshRenderer.GetBlendShapeWeight(msh1);

     /* check for weight/array bounds / iterate / wrap index */
     if (wgt1 >= blendMax || lst1 <= blendSpeed) {
 
       dlog("Maxed blend shape" + msh);

       //       if( init == 1 )
       blst = bldx; //ERR

       if( bldx < blqnt ) {
          lst1 = 0;
          skinnedMeshRenderer.SetBlendShapeWeight(msls, lst1);

          if( bldx == blqnt -1 ) {
              // todo if reshuf[1] == shuf[-1]; reshuffle on collision
              dlog("We are All going to DIE! " + blst);
              reshuf();
              bldx = 0;
              wgt1 = 100;  // 100

              skinnedMeshRenderer.SetBlendShapeWeight(msh1, wgt1);
              // skinnedMeshRenderer.SetBlendShapeWeight(msls, lst1);
          }

          if( init == 1 ) {

            bldx = bldx +1;
            wgt1 = 100;  // 100
            lst1 = 0;
            skinnedMeshRenderer.SetBlendShapeWeight(msh1, wgt1);
//
            skinnedMeshRenderer.SetBlendShapeWeight(msls, lst1);
          } else {
            init = 1;

          }
       } else { 
          bldx = 0;
          init = 0;

       }

       msh1 = shuffle[bldx];
       msls = shuffle[blst];

       //dlog("First Index" + bldx + " Last dex is " + blst);
       wgt1 = skinnedMeshRenderer.GetBlendShapeWeight(msh1);    // current
 
     } else {
     //   Debug.Log("Incramenting blend shape" + msh + " Current weight "  + wgt1);
     //   Debug.Log("Decrementing blend shape" + lst + " Current weight "  + lst1);

        wgt1 += blendSpeed;

  //        if( lst1 > 1 )           // TODO: fix percision
           lst1 -= blendSpeed;

        skinnedMeshRenderer.SetBlendShapeWeight(msh1, wgt1);
        skinnedMeshRenderer.SetBlendShapeWeight(msls, lst1);


     }
  }  

}
