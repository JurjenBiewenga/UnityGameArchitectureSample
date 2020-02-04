using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIPrefabSpawner : MonoBehaviour, IEnumerable
{
    public GameObject Parent;
    public GameObject Prefab;

    public BaseSet Items;

    public UnityEvent OnClick;
    
    private void OnEnable()
    {
        foreach (object item in this)
        {
            GameObject go = GameObject.Instantiate(Prefab, Parent.transform);
            Initialize(go, item, OnClick);
        }
    }

    protected abstract void Initialize(GameObject go, object item, UnityEvent onClick);

    private void OnDisable()
    {
        Action action = () => { };
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            int index = i;
            action += () => GameObject.Destroy(Parent.transform.GetChild(index).gameObject);
        }
        action.Invoke();
    }

    public virtual IEnumerator GetEnumerator()
    {
        return Items.GetEnumerator();
    }
}