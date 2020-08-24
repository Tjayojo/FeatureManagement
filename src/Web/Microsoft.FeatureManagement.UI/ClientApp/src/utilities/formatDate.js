import moment from 'moment';
export const formatDate = (date: string) => {
  if (!date || !moment(date).isValid()){
    return '';
  }
  return moment(date).format('MM/DD/YYYY')
};