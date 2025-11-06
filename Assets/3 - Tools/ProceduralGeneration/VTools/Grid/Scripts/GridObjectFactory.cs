using System.Collections.Generic;
using UnityEngine;

namespace VTools.Grid
{
    public static class GridObjectFactory
    {
        private static readonly Dictionary<GridObjectTemplate, Stack<GridObjectController>> _pool =
            new Dictionary<GridObjectTemplate, Stack<GridObjectController>>();

        private static GridObjectController GetFromPool(GridObjectTemplate template)
        {
            if (_pool.TryGetValue(template, out var stack) && stack.Count > 0)
            {
                var ctrl = stack.Pop();
                ctrl.gameObject.SetActive(true);
                return ctrl;
            }
            return null;
        }

        public static void ReturnToPool(GridObjectController controller)
        {
            if (controller == null || controller.GridObject == null) return;
            var template = controller.GridObject.Template;

            if (!_pool.TryGetValue(template, out var stack))
            {
                stack = new Stack<GridObjectController>(64);
                _pool[template] = stack;
            }

            controller.gameObject.SetActive(false);
            controller.transform.SetParent(null, false);
            stack.Push(controller);
        }

        /// <summary>
        /// Spawn a Grid Object with pooling support.
        /// </summary>
        public static GridObjectController SpawnFrom(GridObjectTemplate template, Transform parent = null, int rotation = 0, Vector3? scale = null)
        {
            var finalScale = scale ?? Vector3.one;

            // Try getting a pooled instance
            var pooled = GetFromPool(template);
            GridObjectController view;
            GridObject data;

            if (pooled != null)
            {
                view = pooled;
                data = view.GridObject;
            }
            else
            {
                // Use _view (prefab) directly
                view = Object.Instantiate(template.View, parent);
                data = template.CreateInstance();
                view.Initialize(data);
            }

            view.ApplyTransform(rotation, finalScale);
            view.Rotate(rotation);
            return view;
        }

        /// <summary>
        /// Spawn object directly on a grid cell.
        /// </summary>
        public static GridObjectController SpawnOnGridFrom(GridObjectTemplate template, Cell cell, Grid grid,
            Transform parent = null, int rotation = 0, Vector3? scale = null)
        {
            var view = SpawnFrom(template, parent, rotation, scale);
            view.AddToGrid(cell, grid, parent);
            cell.AddObject(view);
            return view;
        }
    }
}