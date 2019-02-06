using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mix2App.MachiCon
{
	public interface IMachiConEventHandler : IEventSystemHandler
	{
		void endMovie();
	}
}
