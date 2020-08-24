import React from 'react';
import { any, arrayOf, bool, func, number, shape, string } from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { notificationActionCreator } from '../store/actions';

const Notification = ({ notifications, closeNotification }) => {
  React.useEffect(() => {
    notifications.forEach(notification => {
      const toastId = toast[notification.type](notification.content);
      if (toastId) {
        closeNotification(notification.id);
      }
    });
  }, [notifications, closeNotification]);
  return (
    <ToastContainer
      position="bottom-right"
      autoClose={5000}
      hideProgressBar
      newestOnTop
      closeOnClick
      rtl={false}
      pauseOnFocusLoss
      draggable
      pauseOnHover
    />
  );
};

Notification.propTypes = {
  notifications: arrayOf(shape({
    id: number,
    isOpen: bool,
    content: any,
    type: string
  })).isRequired,
  closeNotification: func.isRequired
};

export default connect(
  ({ notification }) => ({ notifications: notification.notifications }),
  dispatch => bindActionCreators({ closeNotification: notificationActionCreator.close }, dispatch)
)(Notification);