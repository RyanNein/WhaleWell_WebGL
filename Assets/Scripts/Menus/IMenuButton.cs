using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IMenuButton
{
	public void DoClick();

	public void Highlight(bool _on);
}
