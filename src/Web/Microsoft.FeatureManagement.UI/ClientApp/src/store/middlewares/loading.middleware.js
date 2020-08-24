import { loadingActionCreator } from '../actions';

export const loadingMiddleware = store => next => action => {
  if (action.type.endsWith('REQUEST')) {
    store.dispatch(loadingActionCreator.increment());
  } else if (
    action.type.endsWith('REQUEST_SUCCESS') ||
    action.type.endsWith('REQUEST_FAILURE') ||
    action.type.endsWith('REQUEST_ERROR')
  ) {
    store.dispatch(loadingActionCreator.decrement());
  }
  return next(action);
};

