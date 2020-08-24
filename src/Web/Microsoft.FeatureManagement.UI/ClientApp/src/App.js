import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Feature from './components/features/Feature';
import './App.css';

export default () => (
  <Layout>
    <Route exact path='/' component={Home}/>
    <Route exact path='/feature/:id' component={Feature}/>
  </Layout>
);
