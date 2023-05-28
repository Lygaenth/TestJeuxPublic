using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestJeux.API.Services.LightSource;
using TestJeux.Business.ObjectValues;
using TestJeux.Display.ViewModels.Display;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.ViewModels
{
	public class ShaderViewModel : BindableBase
    {
        private readonly ILightSourceService _lightSourceService;
        private ShaderType _shaderType;
        public ShaderType ShaderType { get => _shaderType; set => SetProperty(ref _shaderType, value); }

        private List<ItemViewModel> _items;
        public ObservableCollection<LightSourceViewModel> Sources { get; set; }

        public ShaderViewModel(ILightSourceService lightSourceService)
        {
            _lightSourceService = lightSourceService;
            Sources = new ObservableCollection<LightSourceViewModel>();
            _items = new List<ItemViewModel>();
        }

        public ShaderViewModel(ShaderType shaderType, ILightSourceService lightSourceService, List<ItemViewModel> sources)
            :this(lightSourceService)
        {
			Update(shaderType, sources);
        }

        private void OnItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ItemViewModel.Center) && sender is ItemViewModel item)
            {
                if (Sources.Any(s => s.ID == item.ID))
                    Sources.First(s => s.ID == item.ID).Center = item.Center;
                RaisePropertyChanged(nameof(Sources));
            }
        }

		private void OnSourcePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(LightSourceViewModel.IsLit))
			{
				RaisePropertyChanged(nameof(Sources));
			}
		}

		public void Update(ShaderType shaderType, List<ItemViewModel> items)
        {
            _items.Clear();
            _items.AddRange(items);

            ShaderType = shaderType;
			_lightSourceService.ItemLightChanged -= OnItemLightChanged;

			if (_items.Count == 0)
            {
                foreach (var source in items)
                    source.PropertyChanged -= OnItemPropertyChanged;
            }
            else
                Sources = new ObservableCollection<LightSourceViewModel>();

            foreach (var item in _items)
            {
                var lightDto = _lightSourceService.GetLightSourceState(item.ID);
                if (lightDto == null)
                    continue;

                var source = new LightSourceViewModel(item.ID, lightDto.IsLit, lightDto.Intensity, item.Center);
                Sources.Add(source);
                item.PropertyChanged += OnItemPropertyChanged;
                source.PropertyChanged += OnItemPropertyChanged;
            }

			_lightSourceService.ItemLightChanged += OnItemLightChanged;
		}

        public void Unload()
        {
            foreach(var item in _items)
				item.PropertyChanged -= OnItemPropertyChanged;

			_lightSourceService.ItemLightChanged -= OnItemLightChanged;
        }

		private void OnItemLightChanged(object? sender, LightState e)
		{
            if (sender == null)
                return;

            if (!Int32.TryParse(sender.ToString(), out int itemId))
                return;

            if (!Sources.Any(s => s.ID == itemId))
                return;

            var source = Sources.First(s => s.ID == itemId);
            source.IsLit = e.IsLit;
            source.LightIntensity = e.Intensity;

			RaisePropertyChanged(nameof(Sources));
		}
	}
}
