using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class SelectingBehaviour : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private RaycastHit hit;

    [SerializeField] private GameObject[] SelectedHexagons;

    [SerializeField] private int selectedCount;

    private void Start()
    {
        SelectedHexagons = new GameObject[2];
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (selectedCount == 0)
                {
                    if (SelectedHexagons[0] != null)
                    {
                        ResetGlows();
                    }
                    SelectedHexagons[0] = hit.collider.gameObject;
                    selectedCount++;
                }
                else
                {
                    SelectedHexagons[1] = hit.collider.gameObject;
                    selectedCount = 0;
                }

                hit.collider.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                hit.collider.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
            }
        }
    }

    private void ResetGlows()
    {
        for (int i = 0; i < SelectedHexagons.Length; i++)
        {
            SelectedHexagons[i].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            //SelectedHexagons[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", );
        }
    }
}
