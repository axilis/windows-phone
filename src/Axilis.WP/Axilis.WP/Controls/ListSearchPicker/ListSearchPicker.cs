using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Axilis.WP.Controls
{
    public class ListSearchPicker : ItemsControl
    {
        #region Dependency properties

        public static readonly DependencyProperty PlaceholderTextProperty = DependencyObject<ListSearchPicker>.Register(p => p.PlaceholderText);
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PickerTitleProperty = DependencyObject<ListSearchPicker>.Register(p => p.PickerTitle);
        public string PickerTitle
        {
            get { return (string)GetValue(PickerTitleProperty); }
            set { SetValue(PickerTitleProperty, value); }
        }

        #region Search

        public static readonly DependencyProperty IsSearchEnabledProperty = DependencyObject<ListSearchPicker>.Register(p => p.IsSearchEnabled, new PropertyMetadata(true));
        public bool IsSearchEnabled
        {
            get { return (bool)GetValue(IsSearchEnabledProperty); }
            set { SetValue(IsSearchEnabledProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty = DependencyObject<ListSearchPicker>.Register(p => p.SearchText);
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty IsSearchPersistedProperty = DependencyObject<ListSearchPicker>.Register(p => p.IsSearchPersisted);
        public bool IsSearchPersisted
        {
            get { return (bool)GetValue(IsSearchPersistedProperty); }
            set { SetValue(IsSearchPersistedProperty, value); }
        }

        public static readonly DependencyProperty SearchFunctionProperty = DependencyObject<ListSearchPicker>.Register(p => p.SearchFunction);
        public Func<string, object> SearchFunction
        {
            get { return (Func<string, object>)GetValue(SearchFunctionProperty); }
            set { SetValue(SearchFunctionProperty, value); }
        }

        #endregion

        #region Items

        //public static DependencyProperty SelectedIndexProperty = DependencyObject<ListSearchPicker>.Register(p => p.SelectedIndex);
        //public int SelectedIndex
        //{
        //    get { return (int)GetValue(SelectedIndexProperty); }
        //    set { SetValue(SelectedIndexProperty, value); }
        //}

        public static DependencyProperty SelectedItemProperty = DependencyObject<ListSearchPicker>.Register(p => p.SelectedItem, (d, oldValue, newValue) => d.SetPlaceholderVisibility());
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static DependencyProperty SelectedItemTemplateProperty = DependencyObject<ListSearchPicker>.Register(p => p.SelectedItemTemplate);
        public DataTemplate SelectedItemTemplate
        {
            get { return (DataTemplate)GetValue(SelectedItemTemplateProperty); }
            set { SetValue(SelectedItemTemplateProperty, value); }
        }


        #endregion

        #region Flyout

        public static DependencyProperty FlyoutPresenterStyleProperty = DependencyObject<ListSearchPicker>.Register(p => p.FlyoutPresenterStyle);
        public Style FlyoutPresenterStyle
        {
            get { return (Style)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        public static DependencyProperty FlyoutPlacementProperty = DependencyObject<ListSearchPicker>.Register(p => p.FlyoutPlacement);
        public FlyoutPlacementMode FlyoutPlacement
        {
            get { return (FlyoutPlacementMode)GetValue(FlyoutPlacementProperty); }
            set { SetValue(FlyoutPlacementProperty, value); }
        }

        #endregion


        #endregion

        public ListSearchPicker()
        {
            this.DefaultStyleKey = typeof(ListSearchPicker);
        }

        ContentPresenter _placeholderPresenter;
        ContentPresenter _selectedItemPresenter;
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_placeholderPresenter == null)
                _placeholderPresenter = GetTemplateChild("PlaceholderPresenter") as ContentPresenter;
            if (_selectedItemPresenter == null)
                _selectedItemPresenter = GetTemplateChild("SelectedItemPresenter") as ContentPresenter;

            SetPlaceholderVisibility();
        }

        protected void SetPlaceholderVisibility()
        {
            var isSelected = SelectedItem != null;
            if (_placeholderPresenter != null)
                _placeholderPresenter.Visibility = isSelected ? Visibility.Collapsed : Visibility.Visible;
            if (_selectedItemPresenter != null)
                _selectedItemPresenter.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
