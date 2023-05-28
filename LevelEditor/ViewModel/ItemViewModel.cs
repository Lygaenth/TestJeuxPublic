using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.wpf.ViewModel
{
	public class ItemViewModel : BindableBase
	{
		private int _id;
		public int ID { get => _id; }

		private ItemCode _code;
		public ItemCode Code { get => _code; set => SetProperty(ref _code, value); }

		private Point _startPosition;
		public Point StartPosition { get => _startPosition; set => SetProperty(ref _startPosition, value); }

		private DirectionEnum _orientation;
		public DirectionEnum Orientation { get => _orientation; set => SetProperty(ref _orientation, value); }

		private int _defaultState;
		public int DefaultState { get => _defaultState; set => SetProperty(ref _defaultState, value); }

		public ItemViewModel(int id)
		{
			_id = id;
		}
	}
}
