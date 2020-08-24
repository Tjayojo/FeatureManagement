import { apiPaths } from '../../constants';
import { notificationActionCreator } from './notification.actions';
import { GenericActionCreators } from './genericActionCreators';

const featureActions = {
  GET: 'FEATURE_GET_REQUEST',
  GET_SUCCESS: 'FEATURE_GET_REQUEST_SUCCESS',
  GET_ERROR: 'FEATURE_GET_REQUEST_ERROR',
  GET_BY_ID: 'FEATURE_GET_BY_ID_REQUEST',
  GET_BY_ID_SUCCESS: 'FEATURE_GET_BY_ID_REQUEST_SUCCESS',
  GET_BY_ID_ERROR: 'FEATURE_GET_BY_ID_REQUEST_ERROR',
  CREATE: 'FEATURE_CREATE_REQUEST',
  CREATE_SUCCESS: 'FEATURE_CREATE_REQUEST_SUCCESS',
  CREATE_ERROR: 'FEATURE_CREATE_REQUEST_ERROR',
  UPDATE: 'FEATURE_UPDATE_REQUEST',
  UPDATE_SUCCESS: 'FEATURE_UPDATE_REQUEST_SUCCESS',
  UPDATE_ERROR: 'FEATURE_UPDATE_REQUEST_ERROR'
};

const genericActionCreators = new GenericActionCreators(apiPaths.featureUrl, featureActions);
const featureActionCreator = {
  getFeatures: () => async dispatch => {
    await genericActionCreators.getAll(dispatch);
  },
  getFeatureById: (id: any) => async dispatch => {
    if (!id) {
      dispatch(notificationActionCreator.warning('Feature id is required'));
      return;
    }
    await genericActionCreators.getById(dispatch, id);
  },
  createFeature: (newFeature: Object) => async dispatch => {
    if (!newFeature.name) {
      dispatch(notificationActionCreator.warning('Feature name is required'));
      return;
    }
    await genericActionCreators.create(dispatch, newFeature).then(created => {
      if (created) {
        dispatch(notificationActionCreator.success(`Created feature ${newFeature.name}`));
      } else {
        dispatch(notificationActionCreator.error('Feature creation failed'));
      }
    });
  },
  updateFeature: (feature: Object) => async dispatch => {
    if (!feature) {
      dispatch(notificationActionCreator.warning('Feature is required'));
      return;
    }
    if (!feature.id) {
      dispatch(notificationActionCreator.warning('Feature id is required'));
      return;
    }
    await genericActionCreators.update(dispatch, feature.id, feature).then(updated => {
      if (updated) {
        dispatch(notificationActionCreator.success('Feature updated'));
      } else {
        dispatch(notificationActionCreator.error('Feature update failed'));
      }
    });
  }
};

export { featureActions, featureActionCreator };