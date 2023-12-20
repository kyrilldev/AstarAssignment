using Pathing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class SelectingBehaviour : MonoBehaviour
    {
        [SerializeField] private KeyCode key;
        [SerializeField] private RaycastHit hit;

        [SerializeField] private GameObject[] SelectedHexagons;

        [SerializeField] private int selectedCount;
        private IList<IAStarNode> currentPath;
        private IList<IAStarNode> previousPath;

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
                        selectedCount++;

                        if (SelectedHexagons[0] && SelectedHexagons[1] && selectedCount != 0)
                        {
                            selectedCount = 0;
                            if (currentPath != null)
                            {
                                previousPath = currentPath;
                            }
                            currentPath = AStar.GetPath(SelectedHexagons[0].GetComponent<Node>(), SelectedHexagons[1].GetComponent<Node>());
                            Debug.Log("pathing");

                            foreach (IAStarNode node in currentPath)
                            {
                                node.GameObject().GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                                node.GameObject().GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
                                DynamicGI.UpdateEnvironment();
                            }

                            if (previousPath != null)
                            {
                                foreach (IAStarNode node in previousPath)
                                {
                                    node.GameObject().GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                                }
                            }
                        }
                    }

                    hit.collider.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                    hit.collider.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green);
                    DynamicGI.UpdateEnvironment();
                }
            }
        }

        private void TestPath(IList<IAStarNode> path)
        {
            foreach (IAStarNode node in path)
            {
                print(node.Position() + " is the position of: " + node.ToString());
            }
        }

        private void ResetGlows()
        {
            for (int i = 0; i < SelectedHexagons.Length; i++)
            {
                SelectedHexagons[i].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                //SelectedHexagons[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", );
                DynamicGI.UpdateEnvironment();
            }
        }
    }

}
