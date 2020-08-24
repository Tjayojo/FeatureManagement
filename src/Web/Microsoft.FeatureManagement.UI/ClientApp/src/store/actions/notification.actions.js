import { notificationType } from '../../constants';

export const notificationActions = {
  CONNECTED: 'NOTIFICATION_CONNECTED',
  DISCONNECTED: 'NOTIFICATION_DISCONNECTED',
  OPEN: 'NOTIFICATION_OPEN',
  CLOSE: 'NOTIFICATION_CLOSE',
  CLEAR: 'NOTIFICATION_CLEAR'
};


const openNotification = (content, notificationType) => ({
  type: notificationActions.OPEN,
  notificationType,
  content
});

export const notificationActionCreator = {
  open: (content, notificationType) => openNotification(content, notificationType),
  info: content => openNotification(content, notificationType.info),
  success: content => openNotification(content, notificationType.success),
  warning: content => openNotification(content, notificationType.warning),
  error: content => openNotification(content, notificationType.error),
  primary: content => openNotification(content, notificationType.primary),
  close: id => async (dispatch, getState) => {
    const { notification: { notifications } } = getState();
    const notificationExists = notifications.find(n => n.id === id);
    if (notificationExists) {
      dispatch({ type: notificationActions.CLOSE, id });
    }
  },
  clear: () => ({ type: notificationActions.CLEAR })
};