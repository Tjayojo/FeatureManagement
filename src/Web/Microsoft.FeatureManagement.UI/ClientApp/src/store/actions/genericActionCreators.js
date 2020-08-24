import { isSuccessStatusCode, Requester } from '../../utilities';
import { notificationActionCreator } from './notification.actions';
import { appErrors } from '../../constants';

export class GenericActionCreators {
  constructor(endpoint: string, actions: Object) {
    this.endpoint = endpoint;
    this.actions = actions;
  }

  getAll = async (dispatch: Function) => {
    const { GET, GET_SUCCESS, GET_ERROR } = this.actions;
    dispatch({ type: GET });
    try {
      const { data, status } = await Requester.get(this.endpoint);
      dispatch({
        type: isSuccessStatusCode(status) ? GET_SUCCESS : GET_ERROR,
        payload: data
      });
    } catch (e) {
      dispatch({ type: GET_ERROR, payload: e });
      dispatch(notificationActionCreator.error(appErrors.genericError));
    }
  };

  getById = async (dispatch: Function, id: any) => {
    const { GET_BY_ID, GET_BY_ID_SUCCESS, GET_BY_ID_ERROR } = this.actions;
    dispatch({ type: GET_BY_ID, payload: { id } });
    try {
      const { data, status } = await Requester.get(`${this.endpoint}/${id}`);
      dispatch({
        type: isSuccessStatusCode(status) ? GET_BY_ID_SUCCESS : GET_BY_ID_ERROR,
        payload: data
      });
    } catch (e) {
      dispatch({ type: GET_BY_ID_ERROR, payload: e });
      dispatch(notificationActionCreator.error(appErrors.genericError));
    }
  };

  create = async (dispatch: Function, content: Object) => {
    const { CREATE, CREATE_SUCCESS, CREATE_ERROR } = this.actions;
    dispatch({ type: CREATE });
    try {
      const { data, status } = await Requester.post(this.endpoint, content);
      const isSuccess = isSuccessStatusCode(status);
      dispatch({
        type: isSuccess ? CREATE_SUCCESS : CREATE_ERROR,
        payload: data
      });
      return isSuccess;
    } catch (e) {
      dispatch({ type: CREATE_ERROR, payload: e });
      dispatch(notificationActionCreator.error(appErrors.genericError));
    }
  };

  update = async (dispatch: Function, id: any, content: Object) => {
    const { UPDATE, UPDATE_SUCCESS, UPDATE_ERROR } = this.actions;
    dispatch({ type: UPDATE });
    try {
      const { data, status } = await Requester.put(`${this.endpoint}/${id}`, content);
      const isSuccess = isSuccessStatusCode(status);
      dispatch({
        type: isSuccess ? UPDATE_SUCCESS : UPDATE_ERROR,
        payload: data
      });
      return isSuccess;
    } catch (e) {
      dispatch({ type: UPDATE_ERROR, payload: e });
      dispatch(notificationActionCreator.error(appErrors.genericError));
    }
  };
}