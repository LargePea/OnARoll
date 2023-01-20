using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningManager : MonoBehaviour
{
    [SerializeField, Range(0,0.5f)] private float lightningStrikeRange = 0.25f;
    [SerializeField] Transform _player;
    [SerializeField] GameObject _lightningBolt;

    public void StrikeLighting(float chargeTime)
    {
        if (_player == null) return;

        //convert player position into screen position
        float playerViewportPositionX = Camera.main.WorldToViewportPoint(_player.position).x;

        //get a random strike position around the player
        float strikeRangeMin = Mathf.Clamp01(playerViewportPositionX - lightningStrikeRange);
        float strikeRangeMax = Mathf.Clamp01(playerViewportPositionX + lightningStrikeRange);
        float strikePositionScreen = Random.Range(strikeRangeMin, strikeRangeMax);

        //get direction of random strike position
        Vector3 strikePositionWorld = Camera.main.ViewportToWorldPoint(new Vector3(strikePositionScreen, 1, Camera.main.nearClipPlane));
        Vector3 raycastDirection = (strikePositionWorld - Camera.main.transform.position).normalized;

        //create plane and raycast to cast plane raycast
        Plane lightningPlane = new Plane(Vector3.forward, Vector3.zero);
        Ray ray = new Ray(Camera.main.transform.position, raycastDirection);

        //cast plane raycast to determine how far the depth is
        lightningPlane.Raycast(ray, out float distance);

        //set lightning manager position
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(strikePositionScreen, 1, distance));

        if (_lightningBolt.TryGetComponent(out Lightning lightning))
        {
            lightning.Strike(chargeTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
