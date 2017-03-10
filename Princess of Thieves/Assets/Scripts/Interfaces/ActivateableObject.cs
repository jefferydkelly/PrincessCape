using UnityEngine;
using System.Collections;

public interface ActivateableObject
{
	void Activate();
	void Deactivate();

	bool IsActive { get; }
	float ActivationTime{get;}
	bool IsInverted{get;}
}
