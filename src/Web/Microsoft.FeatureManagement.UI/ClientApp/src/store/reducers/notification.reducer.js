import { notificationActions } from '../actions';

const initialState = { notifications: [] };

export const notificationReducer = (state, action) => {
  state = state || initialState;
  const existingIdenticalNotification = state.notifications
    .find(n => n.type === action.notificationType && n.content === action.content);
  if (existingIdenticalNotification) {
    return state;
  }
  switch (action.type) {
    case notificationActions.OPEN:
      return {
        ...state,
        notifications: [
          ...state.notifications,
          {
            id: state.notifications.length + 1,
            isOpen: true,
            content: action.content,
            type: action.notificationType
          }
        ]
      };    
    case notificationActions.CLOSE:
      return { ...state, notifications: state.notifications.filter(n => n.id !== action.id) };
    case notificationActions.CLEAR:
      return initialState;
    default:
      return state;
  }
};
