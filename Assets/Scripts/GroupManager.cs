using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[ExecuteInEditMode]
public class GroupManager : MonoBehaviour
{
    
    public List<Group> groups = new List<Group>();

    // Start is called before the first frame update
    protected virtual void Start(){}

    // Update is called once per frame
    protected virtual void Update()
    {
        if(!Application.isEditor || Application.isPlaying)
        {
            return;
        }
        groups.ForEach(delegate (Group cs)
        {
            cs.RefreshGroup();
        });

    }

    public void IsolateGroup(string name)
    {
        groups.ForEach(delegate (Group g)
        {
            g.active = false;
            g.RefreshGroup();
        });
        Group isolated = groups.Find(delegate (Group g)
        {
            return g.name == name;
        });
        isolated.active = true;
        isolated.RefreshGroup();
    }

    public void ClearEmpty()
    {
        groups.RemoveAll((g) => { return g.objects.Count == 0; });
        groups.ForEach((g) => {g.objects.RemoveAll((go) => { return go == null; });});
    }

    public bool HasEmpty()
    {
       

        return groups.Find((g) => { return g.objects.Count == 0; }) != null
            || groups.Find((g) => { return g.objects.FindAll((go) => { return go == null; }).Count != 0; }) != null;
    }
}

[System.Serializable]
public class Group
{
    public string name;
    public bool active = true;
    public List<GameObject> objects = new List<GameObject>();
    
    public virtual void RefreshGroup()
    {

        objects.ForEach(delegate (GameObject go)
        {
            if(go == null)
            {
                return;
            }
            go.SetActive(active);
        });
    }

}