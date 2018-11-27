using UnityEngine;

public class NextNode : MonoBehaviour
{
    [SerializeField]
    private NextNode nextNode;

    public NextNode GetNextNode()
    {
        return (nextNode);
    }
}
