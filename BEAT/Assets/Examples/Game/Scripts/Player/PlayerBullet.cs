using AntonioHR.Amusi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    public Vector3 Direction { get; internal set; }
    //public NoteEventBinding Binding { get { return this.binding; } }
    public float speed = 3;

    //Vector3 start;
    //Vector3 target;

    private void Start()
    {
        Debug.Log(Direction);
        GetComponent<Rigidbody>().velocity = Direction * speed;
    }

    //protected override void Init()
    //{
    //    PrepareJump();
    //}

    //protected override void OnNoteStart()
    //{
    //    PrepareJump();
    //}

    //protected override void OnNoteUpdate(float progress)
    //{
    //    transform.position = Vector3.Lerp(start, target, progress);
    //}

    //protected override void OnNoteEnd()
    //{
    //    transform.position = target;
    //}


    //private void PrepareJump()
    //{
    //    start = transform.position;
    //    target = transform.position + Direction;
    //}
}
