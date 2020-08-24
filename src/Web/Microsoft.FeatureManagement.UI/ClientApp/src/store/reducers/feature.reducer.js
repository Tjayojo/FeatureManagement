import { featureActions } from '../actions';

const initialState = { features: [], selectedFeature: {}, isLoading: false };
const featureReducer = (state, action) => {
  state = state || initialState;
  switch (action.type) {
    case featureActions.GET:
    case featureActions.CREATE:
    case featureActions.GET_BY_ID:
      return { ...state, isLoading: true, payload: action.payload };
    case featureActions.GET_SUCCESS:
      return { ...state, features: action.payload, isLoading: false };
    case featureActions.CREATE_SUCCESS:
      return { ...state, features: [...state.features, action.payload], isLoading: false };
    case featureActions.GET_BY_ID_SUCCESS: {
      const inStoreFeature = state.features.find(f => f.id === action.payload.id);
      if (inStoreFeature) {
        return {
          ...state,
          features: [
            ...state.features.filter(f => f.id !== inStoreFeature.id),
            [...inStoreFeature, ...action.payload]
          ]
        };
      }
      return { ...state, features: [...state.features, action.payload], isLoading: false };
    }
    default:
      return state;
  }
};

export { featureReducer };