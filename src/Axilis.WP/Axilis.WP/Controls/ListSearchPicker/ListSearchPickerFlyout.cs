using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace Axilis.WP.Controls
{
    public class ListSearchPickerFlyout : FlyoutBase
    {
        #region Dependency properties

        #region Title

        public static readonly DependencyProperty TitleProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.Title);
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region Search

        public static readonly DependencyProperty IsSearchEnabledProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.IsSearchEnabled, new PropertyMetadata(true));
        public bool IsSearchEnabled
        {
            get { return (bool)GetValue(IsSearchEnabledProperty); }
            set { SetValue(IsSearchEnabledProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.SearchText, new PropertyMetadata(default(string), OnSearchTextChanged));
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }
        private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ListSearchPickerFlyout)d;
            control.RefreshSearch();
        }

        public static readonly DependencyProperty IsSearchPersistedProperty = DependencyObject<ListSearchPicker>.Register(p => p.IsSearchPersisted);
        public bool IsSearchPersisted
        {
            get { return (bool)GetValue(IsSearchPersistedProperty); }
            set { SetValue(IsSearchPersistedProperty, value); }
        }

        public static readonly DependencyProperty SearchFunctionProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.SearchFunction);
        public Func<string, object> SearchFunction
        {
            get { return (Func<string, object>)GetValue(SearchFunctionProperty); }
            set { SetValue(SearchFunctionProperty, value); }
        }

        #endregion

        #region Items

        public static readonly DependencyProperty ItemsSourceProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.ItemsSource);
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static DependencyProperty ItemTemplateProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.ItemTemplate);
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static DependencyProperty ItemTemplateSelectorProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.ItemTemplateSelector);
        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }


        //public static DependencyProperty SelectedIndexProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.SelectedIndex);
        //public int SelectedIndex
        //{
        //    get { return (int)GetValue(SelectedIndexProperty); }
        //    set { SetValue(SelectedIndexProperty, value); }
        //}

        public static DependencyProperty SelectedItemProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.SelectedItem);
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        #endregion

        #region Presenter

        public static DependencyProperty FlyoutPresenterStyleProperty = DependencyObject<ListSearchPickerFlyout>.Register(p => p.FlyoutPresenterStyle);
        public Style FlyoutPresenterStyle
        {
            get { return (Style)GetValue(FlyoutPresenterStyleProperty); }
            set { SetValue(FlyoutPresenterStyleProperty, value); }
        }

        #endregion

        #endregion


        ListSearchPickerFlyoutPresenter _presenter;
        public ListSearchPickerFlyout()
        {
            Opening += ListSearchPickerFlyout_Opening;
            Opened += ListSearchPickerFlyout_Opened;
            Closed += ListSearchPickerFlyout_Closed;
        }

        void ListSearchPickerFlyout_Opening(object sender, object e)
        {
        }
        void ListSearchPickerFlyout_Opened(object sender, object e)
        {
            if (_presenter != null)
                _presenter.SetSearchText(SearchText);

            RefreshSearch();
        }
        void ListSearchPickerFlyout_Closed(object sender, object e)
        {
            //clears search text when flyout closes
            if (!IsSearchPersisted)
                SearchText = null;
        }


        protected override Control CreatePresenter()
        {
            _presenter = new ListSearchPickerFlyoutPresenter(this) { Style = FlyoutPresenterStyle };

            _newItemsSource = ItemsSource;
            _presenter.SetItemsSource(_newItemsSource);

            return _presenter;
        }

        object _newItemsSource;
        protected void RefreshSearch()
        {
            if (ItemsSource == null)
                return;

            _newItemsSource = ItemsSource;

            var search = SearchText;
            if (!String.IsNullOrWhiteSpace(search) && IsSearchEnabled)
            {
                if (SearchFunction != null)
                {
                    _newItemsSource = SearchFunction(search);
                }
                else if (ItemsSource is IEnumerable<string>)
                {
                    var list = ItemsSource as IEnumerable<string>;
                    _newItemsSource = list.Where(i => i != null && i.IndexOf(search, StringComparison.OrdinalIgnoreCase) > -1).ToList();
                }
                else if (ItemsSource is IEnumerable<SimpleKeyValue>)
                {
                    var list = ItemsSource as IEnumerable<SimpleKeyValue>;
                    _newItemsSource = list.Where(i =>
                        (i.Key != null && i.Key.IndexOf(search, StringComparison.OrdinalIgnoreCase) > -1) ||
                        (i.Value != null && i.Value.IndexOf(search, StringComparison.OrdinalIgnoreCase) > -1)
                        ).ToList();
                }
                else if (ItemsSource is IEnumerable<object>)
                {
                    var list = ItemsSource as IEnumerable<object>;
                    _newItemsSource = list.Where(i => i != null && i.ToString().IndexOf(search, StringComparison.OrdinalIgnoreCase) > -1).ToList();
                }
            }

            if (_presenter != null)
                _presenter.SetItemsSource(_newItemsSource);
        }
    }

    public class ListSearchPickerFlyoutPresenter : FlyoutPresenter
    {
        public ListSearchPickerFlyoutPresenter()
        {
            DefaultStyleKey = typeof(ListSearchPickerFlyoutPresenter);
        }
        public ListSearchPickerFlyoutPresenter(ListSearchPickerFlyout flyout)
            : this()
        {
            _flyout = flyout;
        }

        ListSearchPickerFlyout _flyout;
        TextBlock _titlePresenter;
        TextBox _searchTextPresenter;
        //Grid _itemsHostPanel;
        ListView _selector;
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_titlePresenter == null)
                _titlePresenter = GetTemplateChild("TitlePresenter") as TextBlock;
            if (_searchTextPresenter == null)
                _searchTextPresenter = GetTemplateChild("SearchTextPresenter") as TextBox;
            //if (_itemsHostPanel == null)
            //    _itemsHostPanel = GetTemplateChild("ItemsHostPanel") as Grid;
            if (_selector == null)
                _selector = GetTemplateChild("Selector") as ListView;

            if (_flyout != null)
            {
                if (_titlePresenter != null)
                {
                    _titlePresenter.Text = _flyout.Title ?? string.Empty;
                }
                if (_searchTextPresenter != null)
                {
                    _searchTextPresenter.TextChanged -= _searchTextPresenter_TextChanged;
                    if (_flyout.IsSearchEnabled)
                    {
                        _searchTextPresenter.Visibility = Visibility.Visible;
                        _searchTextPresenter.Text = _flyout.SearchText ?? string.Empty;
                        _searchTextPresenter.TextChanged += _searchTextPresenter_TextChanged;
                    }
                    else
                    {
                        _searchTextPresenter.Visibility = Visibility.Collapsed;
                    }
                }
                if (_selector != null)
                {
                    _selector.IsItemClickEnabled = true;
                    _selector.ItemClick -= listView_ItemClick;
                    _selector.ItemClick += listView_ItemClick;

                    _selector.ItemTemplate = _flyout.ItemTemplate;
                    _selector.ItemTemplateSelector = _flyout.ItemTemplateSelector;
                    _selector.ItemsSource = _itemsSource ?? _flyout.ItemsSource;
                }
            }
        }

        object _itemsSource;
        public void SetItemsSource(object itemsSource)
        {
            _itemsSource = itemsSource;
            if (_selector != null)
                _selector.ItemsSource = _itemsSource;
        }
        public void SetSearchText(string searchText)
        {
            if (_searchTextPresenter != null)
            {
                _searchTextPresenter.TextChanged -= _searchTextPresenter_TextChanged;
                _searchTextPresenter.Text = searchText ?? string.Empty;
                _searchTextPresenter.TextChanged += _searchTextPresenter_TextChanged;
            }
        }
        void _searchTextPresenter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tbx = sender as TextBox;
            if (tbx != null && _flyout != null)
                _flyout.SearchText = tbx.Text;
        }
        void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_flyout != null)
            {
                _flyout.SelectedItem = e.ClickedItem;
                _flyout.Hide();
            }
        }
    }

}
