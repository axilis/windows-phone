using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Axilis.WP
{
    public class DependencyObject<TDependencyClass> : DependencyObject
        where TDependencyClass : DependencyObject
    {
        public static DependencyProperty Register<TDependencyPropertyType>(
            Expression<Func<TDependencyClass, TDependencyPropertyType>> propertyExpression)
        {
            return DependencyProperty.Register(
                Member<TDependencyClass>.Name(propertyExpression),
                typeof(TDependencyPropertyType),
                typeof(TDependencyClass),
                new PropertyMetadata(default(TDependencyPropertyType))
                );
        }

        public static DependencyProperty Register<TDependencyPropertyType>(
            Expression<Func<TDependencyClass, TDependencyPropertyType>> propertyExpression,
            Action<TDependencyClass, TDependencyPropertyType, TDependencyPropertyType> callback)
        {
            return DependencyProperty.Register(
                Member<TDependencyClass>.Name(propertyExpression),
                typeof(TDependencyPropertyType),
                typeof(TDependencyClass),
                new PropertyMetadata(default(TDependencyPropertyType),
                    (o, args) =>
                    {
                        if (callback != null)
                            callback((TDependencyClass)o,
                                (TDependencyPropertyType)args.OldValue,
                                (TDependencyPropertyType)args.NewValue);
                    })
                );
        }

        public static DependencyProperty Register<TDependencyPropertyType>(
            Expression<Func<TDependencyClass, TDependencyPropertyType>> propertyExpression,
            PropertyMetadata typeMetadata)
        {
            return DependencyProperty.Register(
                Member<TDependencyClass>.Name(propertyExpression),
                typeof(TDependencyPropertyType),
                typeof(TDependencyClass),
                typeMetadata);
        }
    }
}
