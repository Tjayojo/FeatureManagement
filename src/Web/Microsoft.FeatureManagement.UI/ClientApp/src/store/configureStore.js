import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { connectRouter, routerMiddleware } from 'connected-react-router';
import { featureReducer, loadingReducer, notificationReducer } from './reducers';
import { loadingMiddleware } from './middlewares';

export default function configureStore(history, initialState) {
  const reducers = {
    loading: loadingReducer,
    notification: notificationReducer,
    feature: featureReducer
  };

  const middleware = [routerMiddleware(history), thunk, loadingMiddleware];

  // In development, use the browser's Redux dev tools extension if installed
  const enhancers = [];
  const isDevelopment = process.env.NODE_ENV === 'development';
  if (isDevelopment && typeof window !== 'undefined' && window.__REDUX_DEVTOOLS_EXTENSION__) {
    enhancers.push(window.__REDUX_DEVTOOLS_EXTENSION__(
      {
        trace: true,
        traceLimit: 15
      }
    ));
  }

  const rootReducer = combineReducers({
    router: connectRouter(history),
    ...reducers
  });

  return createStore(
    rootReducer,
    initialState,
    compose(applyMiddleware(...middleware), ...enhancers)
  );
}
