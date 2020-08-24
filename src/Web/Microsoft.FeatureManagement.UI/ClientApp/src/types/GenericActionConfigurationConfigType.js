export type GenericActionConfigurationConfig = {
  endpoint: string,
  actions: {
    GET: string,
    GET_SUCCESS: string,
    GET_ERROR: string
  }
};