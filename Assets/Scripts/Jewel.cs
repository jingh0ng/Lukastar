﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Jewel : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField]
    private Vector3 locationJewel;
    public int identityCode; 
    private Animator anim;
    private Transform jewelChild;
    private Vector3 jewelChildOrigin; //保存当前Jewel的原始位置
    [SerializeField]
    private bool isSelected;
    private IEnumerator moveCoroutine;
    [SerializeField] private bool _isMark;

    private void Start()
    {
        jewelChild = transform.Find("JewelPicture");
        jewelChildOrigin = transform.position;
        anim = jewelChild.GetComponent<Animator>();
        isSelected = false;
        _isMark = false;
        LocationJewelPosition();
    }
    private void Update()
    {
        
    }

    private void LocationJewelPosition()
    {
        locationJewel = GameManager.Instance.GetJewelPosition(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isSelected)
        transform.Find("Select").gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isSelected)
            transform.Find("Select").gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.Instance.canSelect)
        {
            AudioManager.Instance.PlayAudio("Audio/sfx_click");
            //该宝石没有被选择，选中宝石
            if (!isSelected)
            {
                isSelected = true;
                transform.Find("Select").gameObject.SetActive(true);
                if (!JewelsManager.Instance.lastSelected)
                {
                    JewelsManager.Instance.lastSelected = gameObject;
                }
                else if (!JewelsManager.Instance.currentSeleted)
                {
                    JewelsManager.Instance.currentSeleted = gameObject;
                    //调用宝石交换函数
                    JewelsManager.Instance.CallExchangeJewel();
                }
            }
            else if (isSelected)
            {
                isSelected = false;
                transform.Find("Select").gameObject.SetActive(false);
                JewelsManager.Instance.ResetAllJewelSelected();
            }
        }
    }

    //宝石移动的方法
    public void Move(GameObject obj)
    {
        if(moveCoroutine!=null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = MoveCoroutine(obj);
        StartCoroutine(moveCoroutine);
    }

    private IEnumerator MoveCoroutine(GameObject obj)
    {
        GameObject childObj = gameObject.transform.Find("JewelPicture").gameObject;
        childObj.transform.SetParent(obj.transform);
        childObj.transform.DOLocalMove(Vector3.zero, GameManager.Instance.moveDownTime);
        yield return new WaitForSeconds(GameManager.Instance.moveDownTime);
    }

    public bool GetMarkDown()
    {
        if (this._isMark)
        {
            return true;
        }
        return false;
    }

    public void SetMarkDown(bool mark)
    {
        _isMark = mark;
    }

    public void SetSelectedFalse()
    {
        isSelected = false;
    }
}
