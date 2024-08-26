using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileHoverEffect : MonoBehaviour
{
    public LayerMask tileLayer;
    public float hoverHeight = 0.2f;
    public float hoverDuration = 0.3f;
    public float returnDuration = 0.2f;

    private Camera mainCamera;
    private GameObject lastHoveredTile;
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
        {
            GameObject hoveredTile = hit.collider.gameObject;

            if (hoveredTile != lastHoveredTile)
            {
                // Reset the last hovered tile
                if (lastHoveredTile != null)
                {
                    MoveTile(lastHoveredTile, GetOriginalPosition(lastHoveredTile), true);
                }

                // Set the new hovered tile
                lastHoveredTile = hoveredTile;
                Vector3 originalPosition = GetOriginalPosition(hoveredTile);
                Vector3 raisedPosition = originalPosition + Vector3.up * hoverHeight;
                MoveTile(hoveredTile, raisedPosition, false);
            }
        }
        else if (lastHoveredTile != null)
        {
            // If we're not hovering over any tile, reset the last one
            MoveTile(lastHoveredTile, GetOriginalPosition(lastHoveredTile), true);
            lastHoveredTile = null;
        }
    }

    Vector3 GetOriginalPosition(GameObject tile)
    {
        if (!originalPositions.ContainsKey(tile))
        {
            originalPositions[tile] = tile.transform.position;
        }
        return originalPositions[tile];
    }

    void MoveTile(GameObject tile, Vector3 targetPosition, bool isReturning)
    {
        DOTween.Kill(tile.transform);

        float duration = isReturning ? returnDuration : hoverDuration;
        Ease easeType = isReturning ? Ease.InQuad : Ease.OutQuad;

        tile.transform.DOMove(targetPosition, duration)
            .SetEase(easeType)
            .SetUpdate(true);
    }
}