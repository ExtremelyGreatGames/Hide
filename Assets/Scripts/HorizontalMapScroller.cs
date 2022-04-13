using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMapScroller : MonoBehaviour
{
    public List<Transform> _scrollingObjects; // This is kinda expensive. Any other Data Structure?

    // thing that moves it. does it necessarily have to be the player?
    public Transform _playerReference;

    public float _width;

    float _threshold;
    float _centerPositionX;

    private void Start()
    {
        _centerPositionX = transform.position.x;
        _threshold = _width / 2;
    }

    private void Update()
    {
        if (_scrollingObjects.Count <= 1)
        {
            Debug.LogWarning("Not enough scrollable objects");
            return;
        }

        // when a tile region goes out of camera view, move it to the other side

        float playerDistanceToCenter = _playerReference.position.x - _centerPositionX;

        if (playerDistanceToCenter > _threshold)
        {
            // move left most to right most
            var leftMost = _scrollingObjects[0];
            var rightMost = _scrollingObjects[_scrollingObjects.Count - 1];

            var currPos = leftMost.transform.position;
            currPos.x = rightMost.transform.position.x + _width;
            leftMost.transform.position = currPos;

            _scrollingObjects.Remove(leftMost);
            _scrollingObjects.Add(leftMost);

            _centerPositionX += _width;

            Debug.Log($"Swapping");
        }
        else if (playerDistanceToCenter < _threshold * -1)
        {
            // move right most to left most
            var leftMost = _scrollingObjects[0];
            var rightMost = _scrollingObjects[_scrollingObjects.Count - 1];

            var currPos = rightMost.transform.position;
            currPos.x = leftMost.transform.position.x - _width;
            rightMost.transform.position = currPos;

            _scrollingObjects.Remove(rightMost);
            _scrollingObjects.Insert(0, rightMost);

            _centerPositionX -= _width;

            Debug.Log($"Swapping");
        }
    }
}
