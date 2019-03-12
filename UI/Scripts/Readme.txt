How to use.
1) Add UIManager component to UI Canvas root object.
2) Call ShowModal<T>(prefab) to show prefab window.
		Note: prefab must contain component type T
		
Example:
UIManager.ShowModal<ConfirmDialog>(ConfirmDialogPrefab).AddCancelAction(() => {
                Debug.Log("Some cancel action");
            }).AddOKAction(()=> {
                Debug.Log("Some OK action");
            }).AddCloseAction(() => {
                Debug.Log("Some close action");
            });

