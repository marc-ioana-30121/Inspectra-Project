import { createRouter, createWebHistory } from 'vue-router'
import GoodsTable from '../views/GoodsTable.vue'
import GoodsTableReturned from '../views/GoodsTableReturned.vue'
import UploadGoods from '../views/UploadGoods.vue'
import EmployeeTable from '../views/EmployeeTable.vue'
import deliveryReport from '../views/DeliveryReport.vue'
import toDeliver from '../views/ToDeliver.vue'
import QualifiersTab from '../views/QualifiersTab.vue'
import InStock from '../views/InStock.vue'
import PastDelivery from '../views/PastDelivery.vue'
import PastReport from '../views/PastReport.vue'
import EmployeeHours from '../views/EmployeeHours.vue'
const routes = [
  {
    path: '/',
    name: 'goodsTable',
    component: GoodsTable
  },
  {
    path: '/returned',
    name: 'goodsTableReturned',
    component: GoodsTableReturned
  },
  {
    path: '/upload',
    name: 'uploadGoods',
    component: UploadGoods
  },
  {
    path: '/employee',
    name: 'employeeTable',
    component: EmployeeTable
  },
  {
    path: '/DeliveryNote/Valid',
    name: 'deliveryReport',
    component: deliveryReport
  },
  {
    path: '/toDeliver',
    name: 'toDeliver',
    component: toDeliver
  },
  {
    path: '/QualifiersTab',
    name: 'QualifiersTab',
    component: QualifiersTab
  },
  {
    path: '/InStock',
    name: 'InStock',
    component: InStock
  },
  {
    path: '/PastDelivery',
    name: 'PastDelivery ',
    component: PastDelivery
  },
  {
    path: '/PastReport',
    name: 'PastReport',
    component: PastReport
  }
  , {
    path: '/EmployeeHours',
    name: 'EmployeeHours',
    component: EmployeeHours
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
