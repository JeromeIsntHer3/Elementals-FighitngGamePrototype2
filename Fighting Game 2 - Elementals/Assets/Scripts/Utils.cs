using System.Collections;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public delegate void DelayDelegate();

    public static void DestroyChildren(Transform transform)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static IEnumerator Delay(float time, DelayDelegate dele)
    {
        yield return new WaitForSeconds(time);
        dele?.Invoke();
    }

    public static IEnumerator DelayEndFrame(DelayDelegate dele)
    {
        yield return new WaitForEndOfFrame();
        dele?.Invoke();
    }
}