using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] public bool enableBobbing = true;

    [SerializeField, Range(0, .1f)] private float amplitude = .015f;
    [SerializeField, Range(0, 30)] private float frequency = 10f;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform camHolder;

    private float toggleSpeed = 1.5f;
    private Vector3 startPos;
    private CharacterController characterController;

	private void Awake()
	{
        characterController = GetComponent<CharacterController>();
        startPos = cam.localPosition;
	}

    private Vector3 FootStepMotion(float speed)
	{
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency * (speed / 10)) * amplitude * (speed / 10);
        pos.x += Mathf.Cos(Time.time * frequency * (speed / 10) / 2) * amplitude * 2 * (speed / 10);
        return pos;
	}

    private void CheckMotion()
	{
        float speed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
        ResetPosition();

        if (speed < toggleSpeed) return;
        if (!characterController.isGrounded) return;

        PlayMotion(FootStepMotion(speed));
	}

    private void ResetPosition()
	{
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 4 * Time.deltaTime);
	}

    // Update is called once per frame
    void Update()
    {
        if (!enableBobbing) return;

        CheckMotion();
        //cam.LookAt(FocusTarget());
    }

    private Vector3 FocusTarget()
	{
        Vector3 pos = new Vector3(cam.position.x, transform.position.y + camHolder.localPosition.y, transform.position.z);
        pos += camHolder.forward * 15;
        return pos;
	}

    private void PlayMotion(Vector3 motion)
	{
        cam.localPosition += motion;
	}
}
