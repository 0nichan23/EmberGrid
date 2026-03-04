using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeSelections
{
    [SerializeField] private List<int> selectedNodeIndices;

    public TreeSelections()
    {
        selectedNodeIndices = new List<int>();
    }

    public int GetSelection(int lineIndex)
    {
        if (lineIndex < 0 || lineIndex >= selectedNodeIndices.Count)
            return -1;
        return selectedNodeIndices[lineIndex];
    }

    public void SetSelection(int lineIndex, int choiceIndex)
    {
        while (selectedNodeIndices.Count <= lineIndex)
            selectedNodeIndices.Add(-1);
        selectedNodeIndices[lineIndex] = choiceIndex;
    }

    public bool HasSelection(int lineIndex)
    {
        return GetSelection(lineIndex) >= 0;
    }
}
