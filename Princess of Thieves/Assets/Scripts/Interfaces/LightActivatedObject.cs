using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LightActivatedObject {
	void Activate();
	void Deactivate();

	bool IsActive { get; }
}
