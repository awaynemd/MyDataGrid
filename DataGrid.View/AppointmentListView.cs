using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataGrid.View
{
    public interface IVisit { }

    public class ScheduledAppointmentEventArgs : RoutedEventArgs
    {
        public IVisit appointmentKey { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledAppointmentEventArgs"/> class. Constructs a new custom 
        /// ScheduledAppointmentEventArgs object using the parameters provided
        /// </summary>
        /// <param name="routedEvent">The routed event.</param>
        /// <param name="appointmentKey">The scheduled appointment.</param>
        public ScheduledAppointmentEventArgs(RoutedEvent routedEvent, IVisit appointmentKey)
            : base(routedEvent)
        {
            this.appointmentKey = appointmentKey;
        }
    }

    public class AppointmentListView : ListView
    {
        static AppointmentListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppointmentListView), new FrameworkPropertyMetadata(typeof(AppointmentListView)));
        }

        /// <summary>
        /// Constructor. Initializes a new instance of the <see cref="AppointmentListView"/> class.
        /// The AppointmentListView is attached to each cell of the DataGrid.
        /// </summary>
        public AppointmentListView()
        {
            this.Loaded += AppointmentListView_Loaded;
        }

        // Attach the AppointmentListView_PreviewMouseLeftButtonUp to the PreviewMouseLeftButtonUp.
        private void AppointmentListView_Loaded(object sender, RoutedEventArgs e)
        {
            // Must use the mouse to get the selected item from a listview.
            PreviewMouseLeftButtonUp -= AppointmentListView_PreviewMouseLeftButtonUp;
            PreviewMouseLeftButtonUp += AppointmentListView_PreviewMouseLeftButtonUp;
        }

        private void AppointmentListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if ((IVisit)item != null)
            {
               ScheduledAppointmentEventArgs args = new ScheduledAppointmentEventArgs(ScheduledAppointmentEvent, (IVisit)item);
               RaiseEvent(args);
                // This event is caught in Appointments.xaml.cs: AppointmentDataGrid_ScheduledAppointment
            }
        }

        #region [ScheduledAppointmentSelected Bubbling RoutedEvent]
        // To Create a custom routed event with a parameter, create a new subclass of RoutedEventArgs, add a property to it where you can store the 
        // variable to be passed and create a respective handler delegate of type void (object, YourNewEventArgs) which you then assign as the handler 
        // type of your event. If the event then is to be raised you need to create your new event args and pass the variable to its property for 
        // that variable.

        public delegate void ScheduledAppointmentEventHandler(object sender, ScheduledAppointmentEventArgs e);

        // Create a custom routed event by first registering a RoutedEventID. The ScheduledAppointmentSelectedEvent will use custom event args.
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent ScheduledAppointmentEvent = EventManager.RegisterRoutedEvent(
            "ScheduledAppointment", RoutingStrategy.Bubble, typeof(ScheduledAppointmentEventHandler), typeof(AppointmentListView));

        // This is the TARGET of the binding:  customcontrols:AppointmentListView.ScheduledAppointment ="AppointmentDataGrid_ScheduledAppointment"  
        // the ""AppointmentDataGrid_ScheduledAppointment" is the handler of this event.
        // Provide CLR accessors for the event. The RoutedEventHandler is replaced with the new custom delegate.
        public event ScheduledAppointmentEventHandler ScheduledAppointment
        {
            add { AddHandler(ScheduledAppointmentEvent, value); }
            remove { RemoveHandler(ScheduledAppointmentEvent, value); }
        }
        #endregion
    }
}
