// import { defineStore } from 'pinia'

// export const useCartStore = defineStore('cart', {
//   state: () => ({
//     items: JSON.parse(localStorage.getItem('cart')||'[]')
//   }),
//   getters: {
//     total: state =>
//       state.items.reduce((s,i)=>s + i.cijena * i.count, 0).toFixed(2),
//     isEmpty: state => state.items.length===0
//   },
//   actions: {
//     add(proizvod) {
//       const st = this.items.find(x=>x.id===proizvod.id)
//       st ? st.count++ : this.items.push({ ...proizvod, count:1 })
//       localStorage.setItem('cart', JSON.stringify(this.items))
//     },
//     remove(proizvod) {
//       const st = this.items.find(x=>x.id===proizvod.id)
//       if(!st) return
//       st.count--
//       if(st.count===0) this.items = this.items.filter(x=>x.id!==proizvod.id)
//       localStorage.setItem('cart', JSON.stringify(this.items))
//     },
//     clear() {
//       this.items = []
//       localStorage.removeItem('cart')
//     }
//   }
// })
